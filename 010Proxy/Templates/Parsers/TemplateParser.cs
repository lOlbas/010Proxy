using _010Proxy.Network.TCP;
using _010Proxy.Templates.Attributes;
using _010Proxy.Types;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using ProtoBuf;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _010Proxy.Templates.Parsers
{
    public enum ParseMode
    {
        Root,
        Complete
    }

    public struct FieldMeta
    {
        public string Name;
        public object Value;
        public long Offset;
        public long Length;
    }

    public sealed class TemplateParser
    {
        private BinaryReader _br;
        private Assembly _assembly;
        private List<Type> _rootEvents = new List<Type>();
        private Dictionary<object, Type> _eventsMap = new Dictionary<object, Type>(); // Key is OpCode

        private static readonly IEnumerable<string> DefaultNamespaces = new[]
        {
            "_010Proxy.Templates",
            "_010Proxy.Templates.Attributes",
            "System",
            "System.IO",
            "System.Net",
            "System.Linq",
            "System.Text",
            "System.Text.RegularExpressions",
            "System.Collections.Generic"
        };

        private Dictionary<Type, string> _basicTypesParsers = new Dictionary<Type, string>()
        {
            {typeof(bool), "ReadBool"},

            {typeof(byte), "ReadByte"},
            {typeof(sbyte), "ReadSByte"},

            {typeof(char), "ReadChar"},

            {typeof(float), "ReadSingle"},
            {typeof(double), "ReadDouble"},

            {typeof(short), "ReadInt16"},
            {typeof(ushort), "ReadUInt16"},

            {typeof(int), "ReadInt32"},
            {typeof(uint), "ReadUInt32"},

            {typeof(long), "ReadInt64"},
            {typeof(ulong), "ReadUInt64"},
        };

        public void LoadProtocol(RepositoryNode repository)
        {
            if (repository.Type == EntryType.Protocol)
            {
                CompileTemplates(repository);
                ParseAssembly();
            }
        }

        public void CompileTemplates(RepositoryNode repository)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            compilerParams.ReferencedAssemblies.AddRange(new[] { "System.dll", "mscorlib.dll", "System.Core.dll", Assembly.GetEntryAssembly()?.Location, typeof(ProtoContractAttribute).Assembly.Location });

            var provider = new CSharpCodeProvider();
            var usingList = string.Join("", DefaultNamespaces.Select(u => $"using {u}; "));
            var codes = repository.GetFiles().Select(template => usingList + template).ToArray();

            var compile = provider.CompileAssemblyFromSource(compilerParams, codes);

            if (compile.Errors.HasErrors)
            {
                // TODO: conveniently display on form
                var text = compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current + ("rn" + ce.ToString()));

                throw new Exception(text);
            }

            _assembly = compile.CompiledAssembly;
        }

        private bool ParseAssembly()
        {
            foreach (var module in _assembly.GetModules())
            {
                foreach (var type in module.GetTypes())
                {
                    if (type.IsDefined(typeof(RootAttribute)))
                    {
                        _rootEvents.Add(type);
                    }

                    var eventAttributes = type.GetCustomAttributes(typeof(EventAttribute), false);

                    foreach (EventAttribute attribute in eventAttributes)
                    {
                        _eventsMap.Add(attribute.OpCode, type);
                    }
                }
            }

            return _rootEvents.Count != 0;
        }

        private Dictionary<object, object> ReadEvent(out object opCode, ParseMode parseMode = ParseMode.Root)
        {
            opCode = null;
            var packetData = new Dictionary<object, object>();

            foreach (var rootEventType in _rootEvents)
            {
                try
                {
                    var opCodeDataMeta = new FieldMeta();
                    var streamPosition = _br.BaseStream.Position;

                    foreach (var fieldInfo in rootEventType.GetFields())
                    {
                        //if (fieldInfo.FieldType.HasAttribute<FieldAttribute>(out var fieldAttribute))
                        if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                        {
                            var fieldMeta = new FieldMeta
                            {
                                Name = fieldInfo.Name,
                                Offset = _br.BaseStream.Position - streamPosition,
                                Value = ParseField(fieldInfo, fieldAttribute, packetData),
                            };

                            fieldMeta.Length = _br.BaseStream.Position - streamPosition - fieldMeta.Offset;
                            packetData.Add(fieldInfo.Name, fieldMeta);

                            if (fieldAttribute.IsOpCode)
                            {
                                // opCodes.Add(fieldAttribute.OpCodeIndex, eventData[fieldInfo.Name]);

                                try
                                {
                                    // OpCode as described in template might be, for example, of "short" type,
                                    // but if we set "[Event(OpCode=1)]" then typeof(OpCode) will be "int",
                                    // so we have to cast to the expected type or else the lookup will not work.
                                    opCode = Convert.ChangeType(fieldMeta.Value, fieldInfo.FieldType);
                                }
                                catch (Exception e)
                                {
                                    // Fallback to type specified in template, which we can be forced like this:
                                    // "[Event(OpCode=(short)1)]"
                                    opCode = fieldMeta.Value;
                                }
                            }
                            else if (fieldAttribute.ForOpCode != null)
                            {
                                opCodeDataMeta = fieldMeta;
                            }
                        }
                    }

                    if (parseMode == ParseMode.Root)
                    {
                        continue;
                    }

                    if (parseMode == ParseMode.Complete && opCode != null && _eventsMap.TryGetValue(opCode, out var eventType))
                    {
                        var typeFields = eventType.GetFields();

                        if (opCodeDataMeta.Length != 0)
                        {
                            _br.BaseStream.Position = opCodeDataMeta.Offset;
                        }

                        try
                        {
                            foreach (var fieldInfo in typeFields)
                            {
                                if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                                {
                                    var fieldMeta = new FieldMeta
                                    {
                                        Name = fieldInfo.Name,
                                        Offset = _br.BaseStream.Position,
                                        Value = ParseField(fieldInfo, fieldAttribute, packetData),
                                    };

                                    fieldMeta.Length = _br.BaseStream.Position - fieldMeta.Offset;
                                    packetData.Add(fieldInfo.Name, fieldMeta);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _br.BaseStream.Position = opCodeDataMeta.Offset + opCodeDataMeta.Length;
                        }
                    }

                    // Parsing successful
                    break;
                }
                catch (Exception e)
                {
                    // Parsing failed, try next root template
                    continue;
                }
            }

            return packetData;
        }

        // Attempts to parse a single event out of the buffer. Any leftover bytes are ignored.
        public Dictionary<object, object> ParseBuffer(byte[] buffer, ParseMode parseMode = ParseMode.Root)
        {
            _br = new BinaryReader(new MemoryStream(buffer));

            var eventData = ReadEvent(out var opCode, parseMode);

            _br.Dispose();
            _br = null;

            return eventData;
        }

        // TODO: this should better return class instance for the purpose of using its methods, e.g. ToString, but then we lose the FieldMeta descriptions
        public List<ParsedEvent> ParseStream(TcpStream tcpStream, ParseMode parseMode = ParseMode.Root, long position = 0)
        {
            var events = new List<ParsedEvent>();

            if (tcpStream.Length == 0)
            {
                return events;
            }

            _br = new BinaryReader(tcpStream);
            _br.BaseStream.Position = position;

            while (_br.BaseStream.Position < _br.BaseStream.Length)
            {
                var newParsedEvent = new ParsedEvent
                {
                    Offset = tcpStream.Position,
                    Sender = tcpStream.Sender,
                    ParseMode = parseMode
                };
                var packetData = ReadEvent(out var opCode, parseMode);

                newParsedEvent.OpCode = opCode;
                newParsedEvent.Length = tcpStream.Position - newParsedEvent.Offset;
                newParsedEvent.Data = packetData;
                newParsedEvent.Time = tcpStream.PacketAtOffset(tcpStream.Position).Time;

                events.Add(newParsedEvent);
            }

            _br = null;

            return events;
        }

        private object ParseField(FieldInfo fieldInfo, FieldAttribute attribute, Dictionary<object, object> state)
        {
            if (_basicTypesParsers.TryGetValue(fieldInfo.FieldType, out var methodName))
            {
                return typeof(BinaryReader).GetMethod(methodName)?.Invoke(_br, new object[] { });
            }

            if (fieldInfo.FieldType.IsArray)
            {
                if (attribute.SizeField == null)
                {
                    throw new Exception("CountField is missing for byte array.");
                }

                if (state.TryGetValue(attribute.SizeField, out var fieldMeta))
                {
                    var arrayType = fieldInfo.FieldType.GetElementType();

                    if (_basicTypesParsers.TryGetValue(arrayType, out var arrayMethodName))
                    {
                        var readMethod = typeof(BinaryReader).GetMethod(arrayMethodName);
                        dynamic size = ((FieldMeta)fieldMeta).Value; // We assume this is number
                        var arrayValues = Array.CreateInstance(arrayType, size);

                        for (var i = 0; i < size; i++)
                        {
                            dynamic value = readMethod?.Invoke(_br, new object[] { });
                            arrayValues[i] = value;
                        }

                        return arrayValues;
                    }
                }

                throw new Exception("CountField is not parsed yet, can't use it.");
            }

            // TODO: support generic collections

            if (fieldInfo.FieldType.IsEnum)
            {
                if (_basicTypesParsers.TryGetValue(Enum.GetUnderlyingType(fieldInfo.FieldType), out var enumMethodName))
                {
                    var numericValue = typeof(BinaryReader).GetMethod(enumMethodName)?.Invoke(_br, new object[] { });

                    return Enum.ToObject(fieldInfo.FieldType, numericValue);
                }
            }

            // TODO: support array of class

            if (fieldInfo.FieldType.IsClass)
            {
                return ParseClass(fieldInfo.FieldType, attribute, state);
            }

            return null;
        }

        private object ParseClass(Type classType, FieldAttribute attribute, Dictionary<object, object> state)
        {
            if (!classType.IsClass)
            {
                throw new Exception("Expected type to be class.");
            }

            var typeData = new Dictionary<object, object>();

            //if (classType.HasAttribute<ProtoContractAttribute>(out var protoContractAttribute))
            if (classType.GetCustomAttributes(typeof(ProtoContractAttribute), false).FirstOrDefault() is ProtoContractAttribute protoContractAttribute)
            {
                if (attribute.SizeField == null)
                {
                    throw new Exception("CountField is missing for ProtoContract.");
                }

                var protoLength = ((FieldMeta)state[attribute.SizeField]).Value;
                var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 4);
                var genericMethod = method?.MakeGenericMethod(classType);

                return genericMethod?.Invoke(null, new[] { _br.BaseStream, null, null, protoLength });
            }

            return typeData;
        }
    }
}

using _010Proxy.Network.TCP;
using _010Proxy.Templates.Attributes;
using _010Proxy.Types;
using ProtoBuf;
using System;
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

    public sealed class TemplateReader
    {
        private BinaryReader _br;
        private TemplateParser _templateParser;

        private Dictionary<Type, string> _basicTypesReaders = new Dictionary<Type, string>()
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

        public TemplateReader(TemplateParser templateParser)
        {
            _templateParser = templateParser;
        }

        public TemplateReader(TemplateParser templateParser, byte[] buffer) : this(templateParser, new MemoryStream(buffer))
        {
        }

        public TemplateReader(TemplateParser templateParser, Stream stream)
        {
            _templateParser = templateParser;
            _br = new BinaryReader(stream);
        }

        #region Public methods

        // Attempts to parse a single event out of the buffer. Any leftover bytes are ignored.
        public Dictionary<object, object> ParseBuffer(byte[] buffer, ParseMode parseMode = ParseMode.Root)
        {
            _br = new BinaryReader(new MemoryStream(buffer));

            var eventData = ReadEvent(out var opCode, parseMode);

            _br.Dispose();
            _br = null;

            return eventData;
        }

        // TODO: this should probably better return class instance for the purpose of using its methods, e.g. ToString, but then we lose the FieldMeta descriptions
        public List<ParsedEvent> ParseStream(TcpStream tcpStream, ParseMode parseMode = ParseMode.Root, long startingStreamPosition = 0)
        {
            var events = new List<ParsedEvent>();
            var stream = new MemoryStream();

            lock (tcpStream)
            {
                var position = tcpStream.Position;
                tcpStream.Position = startingStreamPosition;
                tcpStream.CopyTo(stream);
                tcpStream.Position = position;
            }

            _br = new BinaryReader(stream);
            _br.BaseStream.Position = 0;

            while (_br.BaseStream.Position < _br.BaseStream.Length)
            {
                try
                {
                    var positionBeforeRead = _br.BaseStream.Position;

                    var newParsedEvent = new ParsedEvent
                    {
                        Offset = positionBeforeRead + startingStreamPosition,
                        Sender = tcpStream.Sender,
                        ParseMode = parseMode
                    };
                    var packetData = ReadEvent(out var opCode, parseMode);

                    if (packetData.Count == 0)
                    {
                        break;
                    }

                    newParsedEvent.OpCode = opCode;
                    newParsedEvent.Length = _br.BaseStream.Position - positionBeforeRead;
                    newParsedEvent.Data = packetData;
                    newParsedEvent.Time = tcpStream.PacketAtOffset(startingStreamPosition + _br.BaseStream.Position).Time;

                    events.Add(newParsedEvent);
                }
                catch (Exception e)
                {
                    // Safe exit
                    break;
                }
            }

            _br.Dispose();
            _br = null;

            return events;
        }

        #endregion

        private Dictionary<object, object> ReadEvent(out object opCode, ParseMode parseMode = ParseMode.Root)
        {
            opCode = null;

            var packetData = new Dictionary<object, object>();
            var streamPosition = _br.BaseStream.Position;
            var rootEvents = _templateParser.RootEvents;

            foreach (var rootEventType in rootEvents)
            {
                try
                {
                    var opCodeDataMeta = new FieldMeta();
                    var rootEventFields = rootEventType.GetFields();

                    foreach (var fieldInfo in rootEventFields)
                    {
                        //if (fieldInfo.FieldType.HasAttribute<FieldAttribute>(out var fieldAttribute))
                        if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                        {
                            if (fieldAttribute.SizeField != null)
                            {
                                // We can continue with other fields if stream has enough data to cover this field size
                                if (packetData.TryGetValue(fieldAttribute.SizeField, out var sizeFieldMeta))
                                {
                                    dynamic dataSize = ((FieldMeta)sizeFieldMeta).Value;

                                    if (_br.BaseStream.Length < _br.BaseStream.Position + dataSize)
                                    {
                                        throw new Exception("Not enough data in the stream.");
                                    }
                                }
                            }

                            var fieldMeta = new FieldMeta
                            {
                                Name = fieldInfo.Name,
                                Offset = _br.BaseStream.Position - streamPosition,
                                Value = ReadField(fieldInfo, fieldAttribute, packetData),
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

                                if (fieldAttribute.SizeField == null)
                                {
                                    throw new Exception("Property \"SizeField\" must be set if field is marked as \"ForOpCode\".");
                                }

                                if (!packetData.TryGetValue(fieldAttribute.SizeField, out var sizeFieldMeta))
                                {
                                    throw new Exception("Property \"SizeField\" should be defined before data. Not the case? - Open issue on GitHub.");
                                }
                            }
                        }
                    }

                    if (parseMode == ParseMode.Root)
                    {
                        if (packetData.Count == rootEventFields.Length)
                        {
                            return packetData;
                        }

                        continue;
                    }

                    if (parseMode == ParseMode.Complete && opCode != null && _templateParser.EventsMap.TryGetValue(opCode, out var eventType))
                    {
                        var typeFields = eventType.GetFields();

                        if (opCodeDataMeta.Length != 0)
                        {
                            _br.BaseStream.Position = opCodeDataMeta.Offset;
                        }

                        try
                        {
                            // Try to parse fields according to templates
                            foreach (var fieldInfo in typeFields)
                            {
                                if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                                {
                                    var fieldMeta = new FieldMeta
                                    {
                                        Name = fieldInfo.Name,
                                        Offset = _br.BaseStream.Position,
                                        Value = ReadField(fieldInfo, fieldAttribute, packetData),
                                    };

                                    fieldMeta.Length = _br.BaseStream.Position - fieldMeta.Offset;
                                    packetData.Add(fieldInfo.Name, fieldMeta);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // On any error we just skip the event data
                            _br.BaseStream.Position = opCodeDataMeta.Offset + opCodeDataMeta.Length;
                        }
                    }

                    // Parsing successful
                    break;
                }
                catch (Exception e)
                {
                    _br.BaseStream.Position = streamPosition;

                    if (rootEvents.Count == 1)
                    {
                        //throw e;
                    }

                    // Parsing failed, try next root template
                    packetData = new Dictionary<object, object>();
                    continue;
                }
            }

            if (packetData.Count == 0)
            {
                // Empty dictionary means we couldn't parse the stream either because of the wrong structure defined in templates or the data is incomplete
                throw new Exception("Failed to read event from stream");
            }

            return packetData;
        }

        private object ReadField(FieldInfo fieldInfo, FieldAttribute attribute, Dictionary<object, object> state)
        {
            if (_basicTypesReaders.TryGetValue(fieldInfo.FieldType, out var methodName))
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

                    if (_basicTypesReaders.TryGetValue(arrayType, out var arrayMethodName))
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
                if (_basicTypesReaders.TryGetValue(Enum.GetUnderlyingType(fieldInfo.FieldType), out var enumMethodName))
                {
                    var numericValue = typeof(BinaryReader).GetMethod(enumMethodName)?.Invoke(_br, new object[] { });

                    return Enum.ToObject(fieldInfo.FieldType, numericValue);
                }
            }

            // TODO: support array of class

            if (fieldInfo.FieldType.IsClass)
            {
                return ReadClass(fieldInfo.FieldType, attribute, state);
            }

            return null;
        }

        private object ReadClass(Type classType, FieldAttribute attribute, Dictionary<object, object> state)
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

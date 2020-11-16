using _010Proxy.Templates.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _010Proxy.Parsers
{
    public sealed class TemplateParser
    {
        private BinaryReader _br;
        private Assembly _assembly;
        private List<Type> _rootEvents = new List<Type>();
        private Dictionary<object, Type> _eventsMap = new Dictionary<object, Type>(); // Key is OpCode

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

        public TemplateParser()
        {

        }

        public bool ParseAssembly(Assembly assembly)
        {
            _assembly = assembly;

            foreach (var module in _assembly.GetModules())
            {
                foreach (var type in module.GetTypes())
                {
                    if (type.IsDefined(typeof(RootAttribute)))
                    {
                        _rootEvents.Add(type);
                    }

                    if (type.GetCustomAttributes(typeof(EventAttribute), false).FirstOrDefault() is EventAttribute attribute)
                    {
                        _eventsMap.Add(attribute.OpCode, type);
                    }
                }
            }

            if (_rootEvents.Count == 0)
            {
                return false;
            }

            return true;
        }

        public Dictionary<object, object> Parse(List<byte> data)
        {
            return Parse(data.ToArray());
        }

        public Dictionary<object, object> Parse(byte[] data)
        {
            var eventData = new Dictionary<object, object>();

            if (data.Length == 0)
            {
                return eventData;
            }

            _br = new BinaryReader(new MemoryStream(data));

            // TODO: support for multi-step opcodes
            // var opCodes = new Dictionary<ushort, object>();
            object opCode = null;

            foreach (var rootEventType in _rootEvents)
            {
                foreach (var fieldInfo in rootEventType.GetFields())
                {
                    //if (fieldInfo.FieldType.HasAttribute<FieldAttribute>(out var fieldAttribute))
                    if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                    {
                        eventData.Add(fieldInfo.Name, ParseField(fieldInfo, fieldAttribute, ref eventData));

                        if (fieldAttribute.IsOpCode)
                        {
                            // opCodes.Add(fieldAttribute.OpCodeIndex, eventData[fieldInfo.Name]);

                            try
                            {
                                // OpCode as described in template might be, for example, of "short" type,
                                // but if we set "[Event(OpCode=1)]" then typeof(OpCode) will be "int",
                                // so we have to cast to the expected type or else the lookup will not work.
                                opCode = Convert.ChangeType(eventData[fieldInfo.Name], fieldInfo.FieldType);
                            }
                            catch (Exception)
                            {
                                // Fallback to type specified in template, which we can force like this:
                                // "[Event(OpCode=(short)1)]"
                                opCode = eventData[fieldInfo.Name];
                            }
                        }
                    }
                }
            }

            if (opCode != null && _eventsMap.TryGetValue(opCode, out var eventType))
            {
                var typeFields = eventType.GetFields();

                foreach (var fieldInfo in typeFields)
                {
                    if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
                    {
                        eventData.Add(fieldInfo.Name, ParseField(fieldInfo, fieldAttribute, ref eventData));
                    }
                }
            }

            return eventData;
        }

        private object ParseField(FieldInfo fieldInfo, FieldAttribute attribute, ref Dictionary<object, object> state)
        {
            if (_basicTypesParsers.TryGetValue(fieldInfo.FieldType, out var methodName))
            {
                return typeof(BinaryReader).GetMethod(methodName)?.Invoke(_br, new object[] { });
            }

            // TODO: support arrays
            if (fieldInfo.FieldType == typeof(byte[])) // fieldInfo.FieldType.IsArray
            {
                if (attribute.CountField == null)
                {
                    throw new Exception("CountField is missing for byte array.");
                }

                if (state.TryGetValue(attribute.CountField, out var value))
                {
                    return _br.ReadBytes(Convert.ToInt32(value));
                }

                throw new Exception("CountField is not");
            }

            if (fieldInfo.FieldType.IsClass)
            {
                return ParseClass(fieldInfo.FieldType, attribute, ref state);
            }

            return null;
        }

        private object ParseClass(Type classType, FieldAttribute attribute, ref Dictionary<object, object> state)
        {
            if (!classType.IsClass)
            {
                throw new Exception("Expected type to be class.");
            }

            var typeData = new Dictionary<object, object>();

            //if (classType.HasAttribute<ProtoContractAttribute>(out var protoContractAttribute))
            if (classType.GetCustomAttributes(typeof(ProtoContractAttribute), false).FirstOrDefault() is ProtoContractAttribute protoContractAttribute)
            {
                if (attribute.CountField == null)
                {
                    throw new Exception("CountField is missing for ProtoContract.");
                }

                var protoLength = state[attribute.CountField];
                var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 4);
                var genericMethod = method?.MakeGenericMethod(classType);

                return genericMethod?.Invoke(null, new[] { _br.BaseStream, null, null, protoLength });
            }

            return typeData;
        }
    }
}

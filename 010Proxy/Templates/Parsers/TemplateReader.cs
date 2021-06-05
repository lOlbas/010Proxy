using _010Proxy.Network.TCP;
using _010Proxy.Templates.Attributes;
using _010Proxy.Types;
using _010Proxy.Utils.Extensions;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _010Proxy.Templates.Parsers
{
    /*
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

        public FieldMeta(string name, long offset, object value = null)
        {
            Name = name;
            Value = value;
            Offset = offset;
            Length = 0;
        }
    }

    /// <summary>
    /// Class instance is intended to be of single use
    /// </summary>
    public sealed class TemplateReader : IDisposable
    {
        private BinaryReader _br;
        private long _eventStartPosition = 0;
        private TemplateParser _templateParser;
        private ParseMode _parseMode;

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

        private Dictionary<string, object> _opCodes = new Dictionary<string, object>();

        public TemplateReader(TemplateParser templateParser, byte[] buffer, ParseMode parseMode)
        {
            _templateParser = templateParser;
            _br = new BinaryReader(new MemoryStream(buffer));
            _parseMode = parseMode;
        }

        public TemplateReader(TemplateParser templateParser, Stream sourceStream, long streamPosition, ParseMode parseMode)
        {
            _templateParser = templateParser;
            var stream = new MemoryStream();

            lock (sourceStream)
            {
                var position = sourceStream.Position;
                sourceStream.Position = streamPosition;
                sourceStream.CopyTo(stream);
                sourceStream.Position = position;
            }

            _br = new BinaryReader(stream);
            _br.BaseStream.Position = 0;
            _parseMode = parseMode;
        }

        #region Public methods

        [Obsolete("Use ParseStream instead.", true)]
        public List<ParsedEvent> ReadEvents()
        {
            var events = new List<ParsedEvent>();

            while (_br.BaseStream.Position < _br.BaseStream.Length)
            {
                try
                {
                    events.Add(ReadEvent());
                }
                catch (Exception e)
                {
                    break;
                }
            }

            return events;
        }

        public ParsedEvent ReadEvent()
        {
            return new ParsedEvent();
        }

        // TODO: this should probably better return class instance for the purpose of using its methods, e.g. ToString, but then we lose the FieldMeta descriptions
        public List<ParsedEvent> ParseStream(ParseMode parseMode = ParseMode.Root, long streamPosition = 0)
        {
            var events = new List<ParsedEvent>();

            while (_br.BaseStream.Position < _br.BaseStream.Length)
            {
                try
                {
                    var positionBeforeRead = _br.BaseStream.Position;

                    var newParsedEvent = new ParsedEvent
                    {
                        Offset = positionBeforeRead + streamPosition,
                        Sender = tcpStream.Sender,
                        ParseMode = parseMode
                    };
                    var packetData = ReadEventNew(parseMode);

                    if (packetData.Count == 0)
                    {
                        break;
                    }

                    newParsedEvent.OpCodes = _opCodes;
                    newParsedEvent.Length = _br.BaseStream.Position - positionBeforeRead;
                    newParsedEvent.Data = packetData;
                    newParsedEvent.Time = tcpStream.PacketAtOffset(streamPosition + _br.BaseStream.Position).Time;

                    events.Add(newParsedEvent);
                }
                catch (Exception e)
                {
                    // Safe exit
                    break;
                }
            }

            return events;
        }

        #endregion

        private ParsedEvent ReadEventNew(ParseMode parseMode = ParseMode.Root)
        {
            _eventStartPosition = _br.BaseStream.Position;

            var rootEvents = _templateParser.RootEvents;
            var eventClasses = new Dictionary<object, object>();
            var packetData = new Dictionary<object, object>();

            foreach (var rootEventType in rootEvents)
            {
                try
                {
                    eventClasses.Add(
                        rootEventType.Name,
                        ReadClassNew(rootEventType, rootEventType.Name, rootEventType.GetCustomAttributes(), packetData)
                    );

                    if (parseMode == ParseMode.Root)
                    {
                        return eventClasses;
                    }

                    if (parseMode == ParseMode.Complete)
                    {

                    }
                }
                catch (Exception e)
                {
                    // Parsing failed, try next root template
                    _br.BaseStream.Position = _eventStartPosition;
                    eventClasses = new Dictionary<object, object>();
                }
            }

            return eventClasses;
        }

        public ParsedEvent ReadEventa(ParseMode parseMode = ParseMode.Root)
        {
            var eventClasses = new Dictionary<object, object>();
            var packetData = new Dictionary<object, object>();
            var streamStartingPosition = _br.BaseStream.Position;
            var rootEvents = _templateParser.RootEvents;

            foreach (var rootEventType in rootEvents)
            {
                try
                {
                    var opCodeDataMeta = new FieldMeta();
                    var rootEventInstance = Activator.CreateInstance(rootEventType);
                    var rootEventFields = rootEventType.GetFields();

                    eventClasses.Add(rootEventType.Name, rootEventInstance);

                    foreach (var fieldInfo in rootEventFields)
                    {
                        if (fieldInfo.HasAttribute<FieldAttribute>(out var fieldAttribute))
                        // if (fieldInfo.GetCustomAttributes(typeof(FieldAttribute), false).FirstOrDefault() is FieldAttribute fieldAttribute)
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
                                Offset = _br.BaseStream.Position - streamStartingPosition,

                            };

                            if (fieldAttribute.Format == FieldFormat.JSON)
                            {
                                fieldMeta.Value = ReadField(fieldInfo, fieldAttribute, packetData);
                            }
                            else
                            {
                                fieldMeta.Value = ReadField(fieldInfo, fieldAttribute, packetData);
                            }

                            fieldMeta.Length = _br.BaseStream.Position - streamStartingPosition - fieldMeta.Offset;

                            fieldInfo.SetValue(rootEventInstance, fieldMeta.Value);

                            packetData.Add(fieldInfo.Name, fieldMeta);

                            if (fieldInfo.HasAttribute<OpCodeAttribute>(out var opCodeAttribute))
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
                            else if (fieldInfo.HasAttribute<OpCodeDataAttribute>(out var opCodeDataAttribute))
                            // else if (fieldAttribute.ForOpCode != null)
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

                    if (parseMode == ParseMode.Complete && _opCodes.Count > 0 && _templateParser.EventsMap.TryGetValue(_opCodes[0], out var eventType))
                    {
                        var typeFields = eventType.GetFields();
                        var eventInstance = Activator.CreateInstance(eventType);

                        if (opCodeDataMeta.Length != 0)
                        {
                            _br.BaseStream.Position = opCodeDataMeta.Offset;
                        }

                        try
                        {
                            // Try to parse fields according to templates
                            foreach (var fieldInfo in typeFields)
                            {
                                if (fieldInfo.GetCustomAttribute<FieldAttribute>() is FieldAttribute fieldAttribute)
                                {
                                    var fieldMeta = new FieldMeta
                                    {
                                        Name = fieldInfo.Name,
                                        Offset = _br.BaseStream.Position,
                                        Value = ReadField(fieldInfo, fieldAttribute, packetData),
                                    };
                                    fieldMeta.Length = _br.BaseStream.Position - fieldMeta.Offset;

                                    packetData.Add(fieldInfo.Name, fieldMeta);

                                    fieldInfo.SetValue(eventInstance, fieldMeta.Value);
                                }
                            }

                            eventClasses.Add(eventType.Name, eventInstance);
                        }
                        catch (Exception e)
                        {
                            // On any error we just skip the event data and keep the event parsed with root template
                            _br.BaseStream.Position = opCodeDataMeta.Offset + opCodeDataMeta.Length;
                        }
                    }

                    // Parsing successful
                    break;
                }
                catch (Exception e)
                {
                    _br.BaseStream.Position = streamStartingPosition;

                    if (rootEvents.Count == 1)
                    {
                        //throw e;
                    }

                    // Parsing failed, try next root template
                    eventClasses = new Dictionary<object, object>();
                    continue;
                }
            }

            if (eventClasses.Count == 0)
            {
                // Empty dictionary means we couldn't parse the stream either because of the wrong structure defined in templates or the data is incomplete
                throw new Exception("Failed to read event from stream");
            }

            return eventClasses;
        }

        private object ReadField(FieldInfo fieldInfo, FieldAttribute attribute, Dictionary<object, object> packetData)
        {
            if (_basicTypesReaders.TryGetValue(fieldInfo.FieldType, out var methodName))
            {
                return typeof(BinaryReader).GetMethod(methodName)?.Invoke(_br, new object[] { });
            }

            if (fieldInfo.FieldType.IsArray)
            {
                // ReadArray();
                if (attribute.SizeField == null)
                {
                    throw new Exception("SizeField is missing for byte array.");
                }

                if (!packetData.TryGetValue(attribute.SizeField, out var fieldMeta))
                {
                    throw new Exception("SizeField is not parsed yet, can't use it.");
                }

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
                return ReadClass(fieldInfo.FieldType, attribute, packetData);
            }

            return null;
        }

        private object ReadClass(Type classType, FieldAttribute attribute, Dictionary<object, object> packetData)
        {
            if (!classType.IsClass)
            {
                throw new Exception("Expected type to be class.");
            }

            var typeData = new Dictionary<object, object>();

            if (classType.HasAttribute<ProtoContractAttribute>(out var protoContractAttribute))
            //if (classType.GetCustomAttributes(typeof(ProtoContractAttribute), false).FirstOrDefault() is ProtoContractAttribute protoContractAttribute)
            {
                if (attribute.SizeField == null)
                {
                    // Try parse without size field
                    var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 1);
                    var genericMethod = method?.MakeGenericMethod(classType);

                    return genericMethod?.Invoke(null, new object[] { _br.BaseStream });
                }
                else
                {

                    var protoLength = ((FieldMeta)packetData[attribute.SizeField]).Value;
                    var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 4);
                    var genericMethod = method?.MakeGenericMethod(classType);

                    return genericMethod?.Invoke(null, new[] { _br.BaseStream, null, null, protoLength });
                }
            }

            return typeData;
        }

        #region Attributes handlers

        //
        private void HandleOpCodeAttribute(OpCodeAttribute opCodeAttribute, FieldInfo fieldInfo, FieldMeta fieldMeta)
        {
            try
            {
                // OpCode as described in template might be, for example, of "short" type,
                // but if we set "[Event(OpCode=1)]" then typeof(OpCode) will be "int",
                // so we have to cast to the expected type or else the lookup will not work.
                _opCodes.Add(opCodeAttribute.Name, Convert.ChangeType(fieldMeta.Value, fieldInfo.FieldType));
            }
            catch (Exception e)
            {
                // Fallback to type specified in template, which we can be forced like this:
                // "[Event(OpCode=(short)1)]"
                _opCodes.Add(opCodeAttribute.Name, fieldMeta.Value);
            }
        }

        //
        private void HandleOpCodeDataAttribute(OpCodeDataAttribute opCodeDataAttribute, FieldAttribute fieldAttribute, Dictionary<object, object> packetData)
        {
            if (fieldAttribute.SizeField == null)
            {
                throw new Exception("Property \"SizeField\" must be set if field is marked as \"ForOpCode\".");
            }

            if (!packetData.TryGetValue(fieldAttribute.SizeField, out var sizeFieldMeta))
            {
                throw new Exception("Property \"SizeField\" should be defined before data. Not the case? - Open issue on GitHub.");
            }
        }

        #endregion

        #region Read methods

        private object ReadClassNew(Type classType, string name, IEnumerable<Attribute> attributes, Dictionary<object, object> packetData)
        {
            if (!classType.IsClass)
            {
                throw new Exception("Expected type to be class.");
            }

            if (classType.HasAttribute<ProtoContractAttribute>(out var protoContractAttribute))
            {
                return ReadProtoClass(classType, name, attributes, packetData);
            }

            var classInstance = Activator.CreateInstance(classType);
            var classFields = classType.GetFields().Where(x => x.IsDefined(typeof(FieldAttribute))).OrderBy(x => x.MetadataToken);

            foreach (var classFieldInfo in classFields)
            {
                var fieldAttribute = classFieldInfo.GetCustomAttribute<FieldAttribute>();
                var fieldMeta = new FieldMeta(classFieldInfo.Name, _br.BaseStream.Position - _eventStartPosition, ReadField(classFieldInfo, fieldAttribute, packetData));

                fieldMeta.Length = _br.BaseStream.Position - _eventStartPosition - fieldMeta.Offset;
                classFieldInfo.SetValue(classInstance, fieldMeta.Value);
                packetData.Add(classFieldInfo.Name, fieldMeta);

                if (classFieldInfo.HasAttribute<OpCodeAttribute>(out var opCodeAttribute))
                {
                    HandleOpCodeAttribute(opCodeAttribute, classFieldInfo, fieldMeta);
                }

                if (classFieldInfo.HasAttribute<OpCodeDataAttribute>(out var opCodeDataAttribute))
                {
                    HandleOpCodeDataAttribute(opCodeDataAttribute, fieldAttribute, packetData);
                }
            }

            return classInstance;
        }

        private object ReadProtoClass(Type classType, string name, IEnumerable<Attribute> attributes, Dictionary<object, object> packetData)
        {
            var fieldAttribute = attributes.OfType<FieldAttribute>().First();

            if (fieldAttribute.SizeField == null)
            {
                var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 1);
                var genericMethod = method?.MakeGenericMethod(classType);

                return genericMethod?.Invoke(null, new object[] { _br.BaseStream });
            }
            else
            {

                var protoLength = ((FieldMeta)packetData[fieldAttribute.SizeField]).Value;
                var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 4);
                var genericMethod = method?.MakeGenericMethod(classType);

                return genericMethod?.Invoke(null, new[] { _br.BaseStream, null, null, protoLength });
            }
        }

        private object ReadDefault(FieldInfo fieldInfo, Dictionary<object, object> packetData)
        {
            if (_basicTypesReaders.TryGetValue(fieldInfo.FieldType, out var methodName))
            {
                return typeof(BinaryReader).GetMethod(methodName)?.Invoke(_br, new object[] { });
            }

            throw new Exception("Not implemented");
        }

        #endregion

        public void Dispose()
        {
            _br?.Dispose();
        }
    }
    */
}

using _010Proxy.Network.TCP;
using _010Proxy.Templates.Attributes;
using _010Proxy.Templates.Parsers;
using _010Proxy.Types;
using _010Proxy.Utils.Extensions;
using BinarySerialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace _010Proxy.Templates
{
    public enum ParseMode
    {
        Root,
        Complete
    }

    public enum ReaderState
    {
        Idle,
        InProgress,
        Done
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

    public class TemplateReader : IDisposable
    {
        public ReaderState State { get; private set; } = ReaderState.Idle;
        public List<string> Errors { get; private set; }

        private BinaryReader _br;
        private long _eventStartPosition = 0;
        private long _streamPosition = 0;
        private TemplateParser _templateParser;
        private ParseMode _parseMode;

        public event Action<float> OnReadEventsProgress;

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
        private Dictionary<object, object> _eventData = new Dictionary<object, object>();

        public TemplateReader(TemplateParser templateParser, byte[] buffer, ParseMode parseMode)
        {
            _templateParser = templateParser;
            _br = new BinaryReader(new MemoryStream(buffer));
            _parseMode = parseMode;
        }

        public TemplateReader(TemplateParser templateParser, Stream sourceStream, long streamPosition, ParseMode parseMode)
        {
            _templateParser = templateParser;
            _streamPosition = streamPosition;

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

        /// <summary>
        /// Returns list of ParsedEvent, where each event represents instance of one of the root templates.
        /// </summary>
        /// <param name="tcpStream"></param>
        /// <returns></returns>
        public List<ParsedEvent> ParseStream(TcpStream tcpStream)
        {
            State = ReaderState.InProgress;

            var events = new List<ParsedEvent>();
            var eventIndex = 1;

            while (_br.BaseStream.Position < _br.BaseStream.Length)
            {
                try
                {
                    var newParsedEvent = new ParsedEvent();

                    newParsedEvent.EventInstance = ReadEvent();

                    newParsedEvent.Offset = _streamPosition + _eventStartPosition;
                    newParsedEvent.Length = _br.BaseStream.Position - _eventStartPosition;

                    newParsedEvent.Sender = tcpStream.Sender;
                    newParsedEvent.Time = tcpStream.PacketAtOffset(_streamPosition + _br.BaseStream.Position).Time;
                    newParsedEvent.ParseMode = _parseMode;

                    newParsedEvent.MetaData = _eventData;
                    newParsedEvent.OpCodes = _opCodes;

                    events.Add(newParsedEvent);
                    eventIndex++;

                    OnReadEventsProgress?.Invoke(_br.BaseStream.Position * 1f / _br.BaseStream.Length);

                    if (newParsedEvent.EventInstance == null) break;
                }
                catch (Exception e)
                {
                    // Errors.Add(e.Message);
                    break;
                }
            }

            State = ReaderState.Done;

            return events;
        }

        /// <summary>
        /// Returns class instance of one of the root templates
        /// </summary>
        /// <returns></returns>
        public object ReadEvent()
        {
            _eventStartPosition = _br.BaseStream.Position;
            _eventData = new Dictionary<object, object>();
            _opCodes = new Dictionary<string, object>();

            foreach (var rootEventType in _templateParser.RootEvents)
            {
                try
                {
                    object instance;

                    if (rootEventType.HasAttribute<JsonAttribute>(out var jsonAttribute))
                    {
                        using (var stream = new MemoryStream())
                        {
                            lock (_br.BaseStream)
                            {
                                var position = _br.BaseStream.Position;
                                _br.BaseStream.CopyTo(stream);
                                _br.BaseStream.Position = position;
                            }

                            stream.Position = 0;

                            using (var sr = new StreamReader(stream, new UTF8Encoding()))
                            {
                                var jsonString = sr.ReadLine();
                                if (jsonString == null)
                                {
                                    continue;
                                }
                                
                                instance = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                                if (instance == null)
                                {
                                    throw new Exception();
                                }

                                _br.BaseStream.Position += jsonString.Length + 1;
                            }
                        }
                    }
                    else
                    {
                        instance = new BinarySerializer().Deserialize(_br.BaseStream, rootEventType);
                        var opCodeFields = rootEventType.GetFields().Where(x => x.IsDefined(typeof(OpCodeAttribute)));

                        foreach (var classFieldInfo in opCodeFields)
                        {
                            _opCodes.Add(classFieldInfo.Name, classFieldInfo.GetValue(instance));
                        }
                    }

                    return instance;
                }
                catch (Exception e)
                {
                    _eventData.Clear();
                    _br.BaseStream.Position = _eventStartPosition;

                    continue;
                }
            }

            _eventData.Clear();

            return null;
        }

        #endregion

        public void Dispose()
        {
            _br?.Dispose();
        }
    }
}

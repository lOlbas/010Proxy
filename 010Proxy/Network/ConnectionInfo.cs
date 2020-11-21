using _010Proxy.Network.TCP;
using _010Proxy.Templates.Parsers;
using _010Proxy.Types;
using _010Proxy.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace _010Proxy.Network
{
    public class ConnectionInfo : INotifyPropertyChanged
    {
        public string AppName { get; }
        public string ProtocolName { get; }
        public IPEndPoint LocalEndPoint { get; }
        public IPEndPoint RemoteEndPoint { get; }

        public bool ServerDetected { get; }

        public List<TimedPacket> Packets { get; }
        public TcpStream ClientStream { get; }
        public TcpStream ServerStream { get; }
        private TemplateParser _templateParser;

        private ulong _totalData;
        private long _lastClientEventPosition;
        private long _lastServerEventPosition;

        public delegate void PacketReceivedHandler(TimedPacket timedPacket);
        public event PacketReceivedHandler OnPacketArrive;

        public delegate void EventsParsedHandler(List<ParsedEvent> parsedEvents);
        public event EventsParsedHandler OnEventsParse;

        public ulong TotalData
        {
            get => _totalData;
            set
            {
                if (_totalData == value) return;
                _totalData = value;
                OnPropertyChanged("TotalData");
            }
        }

        public ConnectionInfo(string appName, string protocolName, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            AppName = appName;
            ProtocolName = protocolName;
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;

            ServerDetected = LocalEndPoint.Address.IsLocal() && !RemoteEndPoint.Address.IsLocal();

            Packets = new List<TimedPacket>();
            ClientStream = new TcpStream(SenderEnum.Client);
            ServerStream = new TcpStream(SenderEnum.Server);
        }

        public void AddPacket(TimedPacket timedPacket)
        {
            lock (Packets)
            {
                Packets.Add(timedPacket);
            }

            TotalData += (ulong)timedPacket.Packet.PayloadData.Length;
            var tcpStream = timedPacket.Sender == SenderEnum.Client ? ClientStream : ServerStream;
            tcpStream.AppendPacket(timedPacket);

            OnPacketArrive?.Invoke(timedPacket);

            TryParseEvents();
        }

        private void TryParseEvents()
        {
            if (_templateParser != null)
            {
                try
                {
                    var parsedEvents = ParseEvents();

                    if (parsedEvents.Count > 0)
                    {
                        OnEventsParse?.Invoke(parsedEvents);
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }

        public void ParseEvents(TemplateParser templateParser)
        {
            _templateParser = templateParser;
            _lastClientEventPosition = 0;
            _lastServerEventPosition = 0;

            TryParseEvents();
        }

        private List<ParsedEvent> ParseEvents()
        {
            if (_templateParser == null)
            {
                return new List<ParsedEvent>();
            }

            var clientEvents = ClientStream.ReadEvents(_templateParser, ParseMode.Root, ref _lastClientEventPosition);
            var serverEvents = ServerStream.ReadEvents(_templateParser, ParseMode.Root, ref _lastServerEventPosition);

            return clientEvents.Concat(serverEvents).OrderBy(pe => pe.Time).ToList();
        }

        public byte[] GetEventBytes(ParsedEvent parsedEvent)
        {
            var sourceStream = parsedEvent.Sender == SenderEnum.Client ? ClientStream : ServerStream;
            var eventBytes = new byte[(int)parsedEvent.Length];

            lock (sourceStream)
            {
                var position = sourceStream.Position;
                sourceStream.Position = parsedEvent.Offset;
                sourceStream.Read(eventBytes, 0, eventBytes.Length);
                sourceStream.Position = position;
            }

            return eventBytes;
        }

        public override string ToString()
        {
            return $"{LocalEndPoint.Address}:{LocalEndPoint.Port} -> {RemoteEndPoint.Address}:{RemoteEndPoint.Port}";
        }

        public override bool Equals(object obj)
        {
            var info = (ConnectionInfo)obj;

            if (info == null)
            {
                return false;
            }

            return EqualsTo(info.ProtocolName, info.LocalEndPoint, info.RemoteEndPoint);
        }

        public bool EqualsTo(string protocolName, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            return ProtocolName == protocolName && LocalEndPoint.Equals(localEndPoint) && RemoteEndPoint.Equals(remoteEndPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = hash * 23 + AppName.GetHashCode();
                hash = hash * 23 + ProtocolName.GetHashCode();
                hash = hash * 23 + LocalEndPoint.GetHashCode();
                hash = hash * 23 + RemoteEndPoint.GetHashCode();

                return hash;
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

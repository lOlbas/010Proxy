using _010Proxy.Templates.Parsers;
using _010Proxy.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _010Proxy.Network.TCP
{
    // Based on https://github.com/chmorgan/packetnet-connections/blob/master/PacketDotNetConnections/TcpStream.cs
    public class TcpStream : MemoryStream
    {
        public SenderEnum Sender { get; }

        private LinkedList<PacketInfo> _packetsInfo;
        private List<TimedPacket> _outOfLoopPackets;
        public List<ParsedEvent> ParsedEvents;

        private readonly object _lock = new object();

        public TcpStream(SenderEnum sender)
        {
            _packetsInfo = new LinkedList<PacketInfo>();
            _outOfLoopPackets = new List<TimedPacket>();
            ParsedEvents = new List<ParsedEvent>();
            Sender = sender;
        }

        public void AppendPacket(TimedPacket newPacket)
        {
            // Assuming the packet is correct for this stream

            var payloadData = newPacket.Packet.PayloadData;
            var payloadLength = payloadData.Length;

            if (payloadLength == 0)
            {
                return;
            }

            lock (_lock)
            {
                if (!_packetsInfo.Any())
                {
                    _packetsInfo.AddFirst(new PacketInfo(newPacket, base.Length));
                }
                // An extra validation step here would be verifying that the order of packets is not messed up, i.e packet #3 did not arrive before packet #2.
                // This approach however does not take into account the client data that change server sequence number,
                // so we rely on the SharpPcap giving us packets in the correct order
                else
                {
                    var newPacketInfo = new PacketInfo(newPacket, base.Length);
                    var position = base.Position;

                    _packetsInfo.AddLast(newPacketInfo);

                    base.Seek(0, SeekOrigin.End);
                    base.Write(payloadData, 0, payloadLength);
                    base.Position = position;
                }
                //else
                //{
                //    _outOfLoopPackets.Add(newPacket);
                //}
            }

            HandleOutOfLoop();
        }

        private void HandleOutOfLoop()
        {
            lock (_packetsInfo)
            {
                foreach (var packet in _outOfLoopPackets)
                {
                    var payloadData = packet.Packet.PayloadData;
                    var payloadLength = payloadData.Length;

                    if (packet.Packet.SequenceNumber == _packetsInfo.Last.Value.Seq + payloadLength)
                    {
                        var newPacketInfo = new PacketInfo(packet, base.Length);
                        var position = base.Position;

                        _packetsInfo.AddLast(newPacketInfo);

                        base.Seek(0, SeekOrigin.End);
                        base.Write(payloadData, 0, payloadLength);
                        base.Position = position;
                    }
                }
            }
        }

        public PacketInfo PacketAtOffset(long offset)
        {
            return _packetsInfo.First(info => info.Offset < offset && info.Offset + info.Length >= offset);
        }

        public void ParseEvents(TemplateParser templateParser, ParseMode parseMode)
        {
            ParsedEvents = templateParser.ParseStream(this, parseMode);
        }
    }
}

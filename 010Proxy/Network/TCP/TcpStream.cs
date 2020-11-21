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

        public TcpStream(SenderEnum sender)
        {
            _packetsInfo = new LinkedList<PacketInfo>();
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

            lock (_packetsInfo)
            {
                // An extra validation step here would be verifying that the order of packets is not messed up, i.e packet #3 did not arrive before packet #2.
                // This approach however does not take into account the client data that change server sequence number,
                // so we rely on the SharpPcap giving us packets in the correct order
                var newPacketInfo = new PacketInfo(newPacket, base.Length);
                lock (this)
                {
                    var position = base.Position;

                    _packetsInfo.AddLast(newPacketInfo);

                    base.Seek(0, SeekOrigin.End);
                    base.Write(payloadData, 0, payloadLength);
                    base.Position = position;
                }
            }
        }

        public PacketInfo PacketAtOffset(long offset)
        {
            return _packetsInfo.First(info => info.Offset < offset && info.Offset + info.Length >= offset);
        }

        public List<ParsedEvent> ReadEvents(TemplateParser templateParser, ParseMode parseMode, ref long startingStreamPosition)
        {
            var parsedEvents = new List<ParsedEvent>();

            if (Length > startingStreamPosition)
            {
                parsedEvents.AddRange(new TemplateReader(templateParser).ParseStream(this, parseMode, startingStreamPosition));

                if (parsedEvents.Count > 0)
                {
                    var lastEvent = parsedEvents.Last();

                    startingStreamPosition = lastEvent.Offset + lastEvent.Length;
                }
            }

            return parsedEvents;
        }
    }
}

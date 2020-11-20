using _010Proxy.Network;
using _010Proxy.Templates.Parsers;
using SharpPcap;
using System.Collections.Generic;

namespace _010Proxy.Types
{
    public class PacketInfo
    {
        public long Ack;
        public long Seq;
        public long Length;
        public long Offset;
        public PosixTimeval Time;

        // Offset is the offset into the stream where this packet starts
        public PacketInfo(TimedPacket timedPacket, long offset)
        {
            Ack = timedPacket.Packet.AcknowledgmentNumber;
            Seq = timedPacket.Packet.SequenceNumber;
            Length = timedPacket.Packet.PayloadData.Length;
            Offset = offset;
            Time = timedPacket.Time;
        }
    }

    public struct ParsedEvent
    {
        public long Offset;
        public long Length;
        public SenderEnum Sender;
        public PosixTimeval Time;
        public object OpCode;
        public ParseMode ParseMode;
        public Dictionary<object, object> Data;
    }
}

using SharpPcap;
using System.Collections.Generic;

namespace _010Proxy.Network.TCP
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

        public PacketInfo(long ack, long seq, long length, long offset, PosixTimeval time)
        {
            Ack = ack;
            Seq = seq;
            Length = length;
            Offset = offset;
            Time = time;
        }
    }

    public sealed class TcpFlow
    {
        public long InitialAck { get; private set; }
        public long InitialSeq { get; private set; }
        public SenderEnum Sender { get; private set; }

        public PosixTimeval FirstPacketTime { get; private set; }
        public PosixTimeval LastPacketTime { get; private set; }

        public List<PacketInfo> PacketsInfo { get; private set; }

        public List<byte> FlowData { get; private set; }

        public TcpFlow()
        {
            PacketsInfo = new List<PacketInfo>();
            FlowData = new List<byte>();
        }

        public void AddPacket(TimedPacket timedPacket)
        {
            if (PacketsInfo.Count == 0)
            {
                FirstPacketTime = timedPacket.Time;
                InitialAck = timedPacket.Packet.AcknowledgmentNumber;
                InitialSeq = timedPacket.Packet.SequenceNumber;
                Sender = timedPacket.Sender;
            }

            lock (PacketsInfo)
            {
                PacketsInfo.Add(new PacketInfo(timedPacket, FlowData.Count));
            }

            lock (FlowData)
            {
                foreach (var value in timedPacket.Packet.PayloadData)
                {
                    FlowData.Add(value);
                }
            }

            LastPacketTime = timedPacket.Time;
        }
    }
}

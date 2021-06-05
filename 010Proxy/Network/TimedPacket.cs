using PacketDotNet;
using SharpPcap;

namespace _010Proxy.Network
{
    public enum SenderEnum
    {
        Unknown,
        Client,
        Server
    }
    public class TimedPacket
    {
        public PosixTimeval Time;
        public RawCapture RawPacket;
        public TcpPacket Packet;
        public SenderEnum Sender;

        public TimedPacket(TcpPacket packet, RawCapture rawPacket, SenderEnum sender)
        {
            Packet = packet;
            RawPacket = rawPacket;
            Time = rawPacket.Timeval;
            Sender = sender;
        }
    }
}

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
        public TcpPacket Packet;
        public SenderEnum Sender;

        public TimedPacket(TcpPacket packet, PosixTimeval time, SenderEnum sender)
        {
            Packet = packet;
            Time = time;
            Sender = sender;
        }
    }
}

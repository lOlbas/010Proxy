using PacketDotNet;
using SharpPcap;
using SharpPcap.Npcap;

namespace _010Proxy.Utils
{
    public static class IPUtils
    {
        public static bool Is<T>(this Packet t, out T outPacket) where T : Packet
        {
            while (t != null)
            {
                if (t is T packet)
                {
                    outPacket = packet;
                    return true;
                }

                t = t.PayloadPacket;
            }

            outPacket = null;

            return false;
        }

        public static string GetFriendlyName(this ICaptureDevice t)
        {
            var friendlyName = t.Name;

            if (t is NpcapDevice nPcap)
            {
                friendlyName = nPcap.Interface.FriendlyName;
            }

            return friendlyName;
        }
    }
}

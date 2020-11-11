using System;

namespace _010Proxy.Utils
{
    public class Time
    {
        // TODO: might be used in packets tables to indicate recent packet time
        public static string ElapsedRelative(long time)
        {
            ulong msPerMinute = 60 * 1000;
            ulong msPerHour = 3600 * 1000;
            ulong msPerDay = 24 * 3600 * 1000;
            var msPerMonth = 30UL * 24 * 3600 * 1000;
            var msPerYear = 365UL * 30 * 24 * 3600 * 1000;

            var elapsed = (ulong)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - time);

            if (elapsed < 2000)
            {
                return "< 1 sec";
            }

            if (elapsed < 5000)
            {
                return "< 5 sec";
            }

            return "Bummer";
        }
    }
}

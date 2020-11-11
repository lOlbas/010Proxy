using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace _010Proxy.Utils.Extensions
{
    public class Subnet
    {
        public IPAddress Address;
        public int MaskLength;

        public Subnet(IPAddress address, int maskLength)
        {
            Address = address;
            MaskLength = maskLength;
        }
    }

    public static class IPAddressExtensions
    {
        // TODO: manage via settings
        public static List<Subnet> Subnets = new List<Subnet>()
        {
            new Subnet(IPAddress.Parse("192.168.0.0"), 16),
        };

        public static int Compare(this IPAddress x, IPAddress y)
        {
            var ipBytes1 = x.GetAddressBytes();
            var ipBytes2 = y.GetAddressBytes();

            for (var i = 0; i < 4; i++)
            {
                if (ipBytes1[i] == ipBytes2[i])
                {
                    continue;
                }

                return ipBytes1[i] - ipBytes2[i];
            }

            return 0;
        }

        public static bool IsLocal(this IPAddress address)
        {
            return Subnets.All(address.IsInSubnet);
        }

        public static bool IsInSubnet(this IPAddress address, Subnet subnet)
        {
            // First parse the address of the netmask before the prefix length.
            var maskAddress = subnet.Address;

            if (address.AddressFamily != maskAddress.AddressFamily)
            { // We got something like an IPV4-Address for an IPv6-Mask. This is not valid.
                return false;
            }

            // Now find out how long the prefix is.
            var maskLength = subnet.MaskLength;

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                // Convert the mask address to an unsigned integer.
                var maskAddressBits = BitConverter.ToUInt32(maskAddress.GetAddressBytes().Reverse().ToArray(), 0);

                // And convert the IpAddress to an unsigned integer.
                var ipAddressBits = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(), 0);

                // Get the mask/network address as unsigned integer.
                uint mask = uint.MaxValue << (32 - maskLength);

                // https://stackoverflow.com/a/1499284/3085985
                // Bitwise AND mask and MaskAddress, this should be the same as mask and IpAddress
                // as the end of the mask is 0000 which leads to both addresses to end with 0000
                // and to start with the prefix.
                return (maskAddressBits & mask) == (ipAddressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // Convert the mask address to a BitArray.
                var maskAddressBits = new BitArray(maskAddress.GetAddressBytes());

                // And convert the IpAddress to a BitArray.
                var ipAddressBits = new BitArray(address.GetAddressBytes());

                if (maskAddressBits.Length != ipAddressBits.Length)
                {
                    throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");
                }

                // Compare the prefix bits.
                for (var maskIndex = 0; maskIndex < maskLength; maskIndex++)
                {
                    if (ipAddressBits[maskIndex] != maskAddressBits[maskIndex])
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new NotSupportedException("Only InterNetworkV6 or InterNetwork address families are supported.");
        }
    }
}

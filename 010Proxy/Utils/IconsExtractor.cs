using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace _010Proxy.Utils
{
    public class IconExtractor
    {

        public static Icon Extract(int number, bool largeIcon = true, string file = "shell32.dll")
        {
            ExtractIconEx(file, number, out var large, out var small, 1);

            try
            {
                return Icon.FromHandle(largeIcon ? large : small);
            }
            catch
            {
                return null;
            }

        }
        [DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);

    }
}

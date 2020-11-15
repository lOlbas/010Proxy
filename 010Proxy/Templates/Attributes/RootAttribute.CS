using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RootAttribute : Attribute
    {
        public ushort Priority { get; set; } = 1;
    }
}

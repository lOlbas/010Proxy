using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EventAttribute : Attribute
    {
        public object OpCode { get; set; }

        public object OpCodeIndex { get; set; }

        public EventAttribute(object opCode = null, ushort opCodeIndex = 1)
        {
            OpCode = opCode;
            OpCodeIndex = opCodeIndex;
        }
    }
}

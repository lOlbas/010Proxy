using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class EventAttribute : Attribute
    {
        public object OpCode { get; set; }

        public object OpCodeIndex { get; set; }

        public string SourceField { get; set; }

        public EventAttribute(object opCode = null, string sourceField = null)
        {
            OpCode = opCode;
            OpCodeIndex = 1;
            SourceField = sourceField;
        }
    }
}

using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RootAttribute : Attribute
    {
        public ushort Priority { get; set; } = 1;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EventAttribute : Attribute
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

    [AttributeUsage(AttributeTargets.Class)]
    public class JsonAttribute : Attribute
    {

    }
}

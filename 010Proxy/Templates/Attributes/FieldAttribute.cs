using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldAttribute : Attribute
    {
        public bool IsOpCode { get; set; }

        public ushort OpCodeIndex { get; set; }

        public string CountField { get; set; }

        public FieldAttribute(bool isOpCode = false, string countField = null)
        {
            IsOpCode = isOpCode;
            CountField = countField;
        }
    }
}

using System;

namespace _010Proxy.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FieldAttribute : Attribute
    {
        public bool IsOpCode { get; set; }

        public ushort OpCodeIndex { get; set; }

        public string SizeField { get; set; }

        public string ForOpCode { get; set; }

        public FieldAttribute(bool isOpCode = false, string sizeField = null, string forOpCode = null)
        {
            IsOpCode = isOpCode;
            SizeField = sizeField;
            ForOpCode = forOpCode;
        }
    }
}

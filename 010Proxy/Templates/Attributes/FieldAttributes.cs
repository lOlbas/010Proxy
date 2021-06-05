using System;
using System.Runtime.CompilerServices;
using BinarySerialization;

namespace _010Proxy.Templates.Attributes
{
    public enum SourceFormat
    {
        Binary,
        Text,
        JSON
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class FieldAttribute : Attribute
    {
        public string SizeField { get; set; }

        public Action<object> SizeFieldModifier { get; set; }

        public SourceFormat Format { get; set; } = SourceFormat.Binary; 

        // public FieldFormat DisplayFormat = ; // Float, binary, hex, double, int, uint, ASCII, UTF8

        public FieldAttribute(string sizeField = null)
        {
            SizeField = sizeField;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class PacketSizeAttribute : FieldAttribute
    {
        public bool FieldSizeIncluded;

        public PacketSizeAttribute(bool fieldSizeIncluded)
        {
            FieldSizeIncluded = fieldSizeIncluded;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ConstAttribute : FieldAttribute
    {
        public object Value { get; set; }

        public ConstAttribute(SourceFormat format, object value)
        {
            Format = format;
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class OpCodeAttribute : FieldAttribute
    {
        public string Name { get; set; }

        public OpCodeAttribute([CallerMemberName] string fieldName = null)
        {
            Name = fieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class OpCodeDataAttribute : FieldAttribute
    {
        public string OpCodeFieldName { get; set; }

        public OpCodeDataAttribute(string opCodeFieldName, string sizeField)
        {
            OpCodeFieldName = opCodeFieldName;
            SizeField = sizeField;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ProtoBufNetFieldAttribute : FieldValueAttributeBase
    {
        public ProtoBufNetFieldAttribute(string valuePath) : base(valuePath)
        {
        }

        protected override object GetInitialState(BinarySerializationContext context)
        {
            return null;
        }

        protected override object GetUpdatedState(object state, byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        protected override object GetFinalValue(object state)
        {
            throw new NotImplementedException();
        }
    }
}

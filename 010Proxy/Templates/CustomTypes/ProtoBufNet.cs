using BinarySerialization;
using System;
using System.IO;

namespace _010Proxy.Templates.CustomTypes
{
    // @Deprecated
    public class ProtoBufNet : IBinarySerializable
    {
        [BinarySerialization.Ignore]
        public byte Value;

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            stream.WriteByte(Value);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var readByte = stream.ReadByte();
            if (readByte == -1) throw new EndOfStreamException();
            Value = Convert.ToByte(readByte);
        }
    }
}

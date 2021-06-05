using BinarySerialization;
using ProtoBuf;
using System;
using System.IO;
using System.Linq;

namespace _010Proxy.Templates
{
    public class ProtoSerializer : CustomSerializer
    {
        public override void Serialize(BoundedStream stream, object value)
        {
            var type = value.GetType();
            var attributes = type.GetCustomAttributes(true).ToList();
            var protoAttribute = attributes.OfType<ProtoContractAttribute>().FirstOrDefault();

            if (protoAttribute == null)
            {
                throw new NotSupportedException("Could not serialized class as ProtoContract: missing attribute.");
            }

            Serializer.Serialize(stream, value);
        }

        public override object Deserialize(BoundedStream stream, Type type)
        {
            var attributes = type.GetCustomAttributes(true).ToList();
            var protoAttribute = attributes.OfType<ProtoContractAttribute>().FirstOrDefault();

            if (protoAttribute == null)
            {
                throw new NotSupportedException("Could not serialized class as ProtoContract: missing attribute.");
            }

            var pos = stream.Position;
            var data = new byte[stream.MaxLength.ByteCount];

            stream.Read(data, 0, (int) stream.MaxLength.ByteCount);

            stream.Position = pos;

            using (var memStream = new MemoryStream(data))
            {
                var method = typeof(Serializer).GetMethods().FirstOrDefault(m => m.Name == "Deserialize" && m.GetParameters().Length == 1);
                var genericMethod = method?.MakeGenericMethod(type);

                try
                {
                    //var obj = genericMethod?.Invoke(null, new object[] { memStream.ToArray() });
                    var obj = Serializer.Deserialize(type, memStream);

                    stream.Position += memStream.Position;

                    if (stream.Position != stream.Length)
                    {

                    }

                    return obj;
                }
                catch (Exception e)
                {
                    //throw new Exception("Error reading Proto data!");
                    return null;
                }
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packets");
            File.WriteAllBytes(path + $"/{DateTimeOffset.Now.ToUnixTimeMilliseconds()}-test.txt", data);
        }
    }
}

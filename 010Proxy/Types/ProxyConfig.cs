using _010Proxy.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace _010Proxy.Types
{
    [Serializable]
    public sealed class ProxyConfig
    {
        public List<ApplicationNode> Applications = new List<ApplicationNode>();
    }

    [Serializable]
    public sealed class ApplicationNode
    {
        public string Name;
        public bool NodeOpened = true;

        public List<IPNode> IPs = new List<IPNode>();
        public List<ProtocolNode> Protocols = new List<ProtocolNode>();
    }

    [Serializable]
    public sealed class IPNode
    {
        public string Name = "";
        public string IPAddress;
        public bool NodeOpened = true;

        public List<ConfigIPPortNode> PortNodes = new List<ConfigIPPortNode>();
    }

    [Serializable]
    public sealed class ConfigIPPortNode
    {
        public string Name;
        public ushort Port;
        // Protocol: TCP, UDP?
        public bool NodeOpened = true;
        public ProtocolNode Protocol = null;
    }

    [Serializable]
    public sealed class ProtocolNode
    {
        public string Name = "";
        public bool NodeOpened = true;

        public Dictionary<object, TemplateNode> Templates = new Dictionary<object, TemplateNode>();
    }

    [Serializable]
    public sealed class TemplateNode
    {
        public string Name = "";
        public object EventType = null;
        public string Code = "";

        public bool IsEmpty()
        {
            return Code == "";
        }
    }

    public class ConfigManager : Singleton<ConfigManager>
    {
        private string _filePath = "config.prx";

        private ConfigManager()
        {
            Load();
        }

        public ProxyConfig Config { get; private set; }

        public void Reload()
        {
            Load();
        }

        public void Save()
        {
            using (Stream stream = File.Open(_filePath, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                binaryFormatter.Serialize(stream, Config);
            }
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                using (Stream stream = File.Open(_filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    Config = (ProxyConfig)binaryFormatter.Deserialize(stream);
                }
            }
            else
            {
                Config = new ProxyConfig();
            }
        }
    }
}

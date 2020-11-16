using _010Proxy.Utils;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace _010Proxy.Types
{
    #region Helpers

    [Serializable]
    public class ConfigNode
    {
        public string Name = "";
        public bool NodeOpened = true;

        public int IconIndex = 0;
        public int OpenedIconIndex = 0;
    }

    [Serializable]
    public enum EntryType
    {
        Protocol,
        Folder,
        File,
        Template
    }

    [Serializable]
    public class RepositoryNode : ConfigNode
    {
        public EntryType Type;

        public RepositoryNode Container = null;
        public List<RepositoryNode> Items = new List<RepositoryNode>();

        public string Content = null;

        public RepositoryNode(EntryType type, string name = "", RepositoryNode container = null, string content = null)
        {
            Type = type;
            Name = name;
            Container = container;
            Content = type == EntryType.Template ? content : null;
            IconIndex = type == EntryType.Protocol || type == EntryType.Folder ? 0 : 1;
        }

        public RepositoryNode AddItem(EntryType type, string name = "", string content = null)
        {
            var newItem = new RepositoryNode(type, name, this, content);
            Items.Add(newItem);

            return newItem;
        }

        public bool CanHaveFiles()
        {
            return Type == EntryType.Folder || Type == EntryType.Protocol;
        }

        public List<string> GetFiles()
        {
            var files = new List<string>();

            foreach (var child in Items)
            {
                if (child.CanHaveFiles())
                {
                    files.AddRange(child.GetFiles());
                }

                if (child.Content != null)
                {
                    files.Add(child.Content);
                }
            }

            return files;
        }

        public List<string> PathTo()
        {
            var path = new List<string>();

            var container = Container;

            while (container != null)
            {
                path.Insert(0, container.Name);

                container = container.Container;
            }

            return path;
        }
    }

    #endregion

    [Serializable]
    public sealed class ProxyConfig
    {
        public List<ApplicationNode> Applications = new List<ApplicationNode>();
    }

    [Serializable]
    public sealed class ApplicationNode : ConfigNode
    {
        public List<RepositoryNode> Protocols = new List<RepositoryNode>();
        public List<IPNode> IPs = new List<IPNode>();
    }

    [Serializable]
    public sealed class IPNode : ConfigNode
    {
        public string IPAddress;

        public List<IPPortNode> Ports = new List<IPPortNode>();
    }

    [Serializable]
    public sealed class IPPortNode : ConfigNode
    {
        public ushort Port;
        public ProtocolType ProtocolType;
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

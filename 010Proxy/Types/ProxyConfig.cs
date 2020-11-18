using _010Proxy.Utils;
using PacketDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        public List<string> GetNamespace()
        {
            var path = new List<string> { Name };

            var container = Container;

            while (container != null)
            {
                path.Insert(0, container.Name);

                container = container.Container;
            }

            return path;
        }

        public bool TryGetFolder(string folder, out RepositoryNode folderNode)
        {
            folderNode = null;

            foreach (var item in Items.Where(item => item.Type == EntryType.Folder && item.Name == folder))
            {
                folderNode = item;
                return true;
            }

            return false;
        }

        public bool TryGetFile(string file, out RepositoryNode fileNode)
        {
            fileNode = null;

            foreach (var item in Items.Where(item => item.Type == EntryType.Template && item.Name == file))
            {
                fileNode = item;
                return true;
            }

            return false;
        }

        public void ExportTo(string path)
        {
            if (Type == EntryType.Folder || Type == EntryType.Protocol)
            {
                var dirInfo = Directory.CreateDirectory(Path.Combine(path, Name));

                foreach (var item in Items)
                {
                    item.ExportTo(dirInfo.FullName);
                }
            }
            else if (Type == EntryType.Template)
            {
                if (File.Exists(path))
                {
                    // TODO: handle overwriting
                }

                File.WriteAllText(Path.Combine(path, Name) + ".cs", Content);
            }
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

    public class ConfigNodeSorter : IComparer
    {
        public ConfigNodeSorter() { }

        public int Compare(object x, object y)
        {
            if (!(x is TreeNode tx) || !(y is TreeNode ty))
            {
                return 0;
            }

            if (tx.Tag is RepositoryNode txRepo && ty.Tag is RepositoryNode tyRepo)
            {
                if (txRepo.Type != tyRepo.Type)
                {
                    return txRepo.Type - tyRepo.Type;
                }
            }

            return string.CompareOrdinal(tx.Text, ty.Text);
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

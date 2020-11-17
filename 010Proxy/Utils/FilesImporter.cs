using _010Proxy.Types;
using System.Collections.Generic;
using System.IO;

namespace _010Proxy.Utils
{
    public enum OverwriteAction
    {
        OverwriteAll,
        SkipAll,
        PromptEvery
    }

    public class ImportNode
    {
        public string Path;
        public string Name;
        public bool FileExistsInConfig;
        public EntryType Type;
        public int TotalConflicts;

        public ImportNode()
        {
        }

        public ImportNode(string path, string name, EntryType type, bool fileExistsInConfig = false)
        {
            Path = path;
            Name = name;
            Type = type;
            FileExistsInConfig = fileExistsInConfig;
        }

        public List<ImportNode> SubNodes = new List<ImportNode>();
    }

    public class FilesImporter
    {
        public static ImportNode FilesToTree(string[] files, RepositoryNode target)
        {
            var tree = new ImportNode(target.Name, target.Name, EntryType.Folder);

            foreach (var file in files)
            {
                var newNode = PathToTree(file, target);

                tree.SubNodes.Add(newNode);
                tree.TotalConflicts += newNode.TotalConflicts;
            }

            return tree;
        }

        public static ImportNode PathToTree(string path, RepositoryNode target = null)
        {
            var attributes = File.GetAttributes(path);
            var filename = Path.GetFileNameWithoutExtension(path);
            var tree = new ImportNode() { Path = path, Name = filename };

            if (attributes.HasFlag(FileAttributes.Directory))
            {
                tree.Type = EntryType.Folder;

                RepositoryNode folderNode = null;
                target?.TryGetFolder(filename, out folderNode);

                foreach (var file in Directory.GetFiles(path))
                {
                    var newNode = PathToTree(file, folderNode);

                    tree.SubNodes.Add(newNode);

                    if (newNode.FileExistsInConfig)
                    {
                        tree.TotalConflicts++;
                    }
                }
            }
            else
            {
                tree.Type = EntryType.Template;

                RepositoryNode fileNode = null;
                target?.TryGetFile(filename, out fileNode);

                if (fileNode != null)
                {
                    tree.FileExistsInConfig = true;
                    tree.TotalConflicts++;
                }
            }

            return tree;
        }
    }
}

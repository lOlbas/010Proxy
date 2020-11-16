using _010Proxy.Network;
using _010Proxy.Types;
using _010Proxy.Utils;
using _010Proxy.Views;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace _010Proxy.Forms
{
    public partial class MainForm : Form
    {
        public Sniffer Sniffer;

        private ConfigManager _configManager;

        public MainForm()
        {
            InitializeComponent();

            Sniffer = new Sniffer();
            _configManager = ConfigManager.Instance;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadImages();
            LoadConfig();
            CreateWelcomeTab();
        }

        private void LoadImages()
        {
            configImageList.Images.Add(IconExtractor.Extract(4));
            configImageList.Images.Add(IconExtractor.Extract(70));
        }

        private void LoadConfig()
        {
            configView.Nodes.Clear();

            var config = _configManager.Config;

            foreach (var app in config.Applications)
            {
                var newAppNode = new TreeNode(app.Name, 0, 0) { Tag = app };

                foreach (var repository in app.Protocols)
                {
                    newAppNode.Nodes.Add(RepositoryToTreeNodes(repository));
                }

                // TODO: do not display IPs in config view, manage them in a separate window

                if (app.NodeOpened)
                {
                    newAppNode.Expand();
                }

                configView.Nodes.Add(newAppNode);
            }
        }

        private TreeNode RepositoryToTreeNodes(RepositoryNode repository)
        {
            var node = new TreeNode(repository.Name, repository.IconIndex, repository.IconIndex) { Tag = repository };

            foreach (var childRepository in repository.Items)
            {
                node.Nodes.Add(RepositoryToTreeNodes(childRepository));
            }

            if (repository.NodeOpened)
            {
                node.Expand();
            }

            return node;
        }

        public void SaveConfig()
        {
            _configManager.Save();
        }

        #region Tabs creation

        private void CreateWelcomeTab()
        {
            var newTab = new TabPage("Welcome");
            var newControl = new HomeControl
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);
            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);
            newControl.OnShow();
        }

        private void CreateCaptureTrafficTab(ICaptureDevice device)
        {
            var newTab = new TabPage($"Capturing from {device.GetFriendlyName()}");
            var newControl = new CaptureTrafficControl()
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);

            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);

            newControl.OnShow();
            newControl.StartNetworkAnalysis(device);

            // captureTrafficControl.OnStateChange += this.OnTabControlStateChange;
        }

        /*
        private void OnTabControlStateChange(TabType type, )
        {
            
        }
        */

        private void CreateConnectionInfoTab(ConnectionInfo connectionInfo)
        {
            var newTab = new TabPage($"{connectionInfo}");
            var newControl = new TcpConnectionInfoControl()
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);
            newTab.Tag = connectionInfo;

            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);

            newControl.LoadPackets(connectionInfo);
        }

        private void CreateConnectionFlowTab(ConnectionInfo connectionInfo)
        {
            var newTab = new TabPage($"{connectionInfo}");
            var newControl = new TcpReconstructedInfoControl()
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);
            newTab.Tag = connectionInfo;

            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);

            newControl.LoadFlows(connectionInfo);
        }

        private void CreateTemplateEditorTab(RepositoryNode repositoryNode)
        {
            var newTab = new TabPage($"{repositoryNode.Name}");
            var newControl = new TemplateEditorControl()
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);
            newTab.Tag = repositoryNode;

            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);

            newControl.LoadTemplate(repositoryNode);
        }

        #endregion

        #region Tabs callbacks

        public void StartNetworkAnalysis(ICaptureDevice device)
        {
            CreateCaptureTrafficTab(device);
        }

        public void ShowConnectionPackets(ConnectionInfo connectionInfo)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Tag is ConnectionInfo tabConnectionInfo)
                {
                    if (Equals(tabConnectionInfo, connectionInfo))
                    {
                        // TODO: show prompt to notify user tab with this connection info already exists and whether he wants to still open new tab
                        tabControl.SelectedTab = tabPage;
                        return;
                    }
                }
            }

            CreateConnectionInfoTab(connectionInfo);
        }

        public void ShowConnectionFlows(ConnectionInfo connectionInfo)
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                if (tabPage.Tag is ConnectionInfo tabConnectionInfo) // TODO: also check control type
                {
                    if (Equals(tabConnectionInfo, connectionInfo))
                    {
                        // TODO: show prompt to notify user tab with this flows info already exists and whether he wants to still open new tab
                        tabControl.SelectedTab = tabPage;
                        return;
                    }
                }
            }

            CreateConnectionFlowTab(connectionInfo);
        }

        public void CreateFlowDataPreview(Dictionary<object, object> data)
        {
            flowDataView.Nodes.Clear();

            if (data == null)
            {
                return;
            }

            foreach (var node in DictionaryToTreeViewNodes(data))
            {
                flowDataView.Nodes.Add(node);
            }
        }

        private IEnumerable<TreeNode> DictionaryToTreeViewNodes(Dictionary<object, object> data)
        {
            var nodes = new List<TreeNode>();

            foreach (var item in data)
            {
                TreeNode newNode;

                if (item.Value is Dictionary<object, object> childData)
                {
                    newNode = new TreeNode($"{item.Key}");
                    foreach (var node in DictionaryToTreeViewNodes(childData))
                    {
                        newNode.Nodes.Add(node);
                    }
                }
                else if (item.Value.GetType().IsClass)
                {
                    newNode = new TreeNode($"{item.Key}");

                    foreach (var node in DictionaryToTreeViewNodes(item.Value.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Select(pi => new { pi.Name, Value = pi.GetValue(item.Value, null) })
                        .Union(
                            item.Value.GetType()
                                .GetFields()
                                .Select(fi => new { fi.Name, Value = fi.GetValue(item.Value) })
                        )
                        .ToDictionary(ks => (object)ks.Name, vs => vs.Value)))
                    {
                        newNode.Nodes.Add(node);
                    }
                }
                else
                {
                    newNode = new TreeNode($"{item.Key} = {item.Value}");
                }

                newNode.Expand();

                nodes.Add(newNode);
            }

            return nodes;
        }

        #endregion

        #region Form handlers

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sniffer.StopCapture();
            _configManager.Save();
        }

        private void tabControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (e.Control)
                    {
                        var selectedIndex = tabControl.SelectedIndex;

                        if (tabControl.SelectedTab.Controls[0] is HomeControl)
                        {
                            break;
                        }

                        ((ProxyTabControl)tabControl.SelectedTab.Controls[0]).OnClose();
                        tabControl.TabPages.Remove(tabControl.SelectedTab);

                        if (selectedIndex > 0)
                        {
                            tabControl.SelectedIndex = selectedIndex - 1;
                            ((ProxyTabControl)tabControl.SelectedTab.Controls[0]).OnShow();
                        }
                    }
                    break;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            // ((ProxyTabControl)e.TabPage.Controls[0]).OnHide();
        }

        private void configView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            configView.SelectedNode = configView.GetNodeAt(e.X, e.Y);
        }

        private void configView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OnOpenTemplateMenuClick(sender, e);
        }

        private void configView_AfterExpandOrCollapse(object sender, TreeViewEventArgs e)
        {
            if (configView.SelectedNode == null)
            {
                return;
            }

            ((ConfigNode)configView.SelectedNode.Tag).NodeOpened = e.Node.IsExpanded;
        }

        private void configView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Label))
            {
                // TODO: somehow forbid same names for nodes

                ((ConfigNode)e.Node.Tag).Name = e.Label;

                e.Node.EndEdit(false);

                _configManager.Save();
            }
            else
            {
                e.Node.EndEdit(true);
            }
        }

        #endregion

        private void CreateConfigContextMenu(object sender, MouseEventArgs e)
        {
            configView.SelectedNode = configView.GetNodeAt(e.X, e.Y);

            if (e.Button == MouseButtons.Right)
            {
                /*
                TODO: Memory leak when creating new context menu
                while (contextMenuStrip.Items.Count > 0)
                {
                    var item = contextMenuStrip.Items[contextMenuStrip.Items.Count - 1];
                    contextMenuStrip.Items.RemoveAt(contextMenuStrip.Items.Count - 1);

                    item.Dispose();
                }
                
                contextMenuStrip.Dispose();
                contextMenuStrip = null;
                contextMenuStrip = new ContextMenuStrip(this.components);
                */

                contextMenuStrip.Items.Clear();

                if (configView.SelectedNode == null)
                {
                    contextMenuStrip.Items.Add(new ToolStripMenuItem("Create Application", null, OnCreateApplicationMenuClick));
                }
                else
                {
                    var canApplyProtocol = tabControl.SelectedTab.Controls[0] is TcpReconstructedInfoControl;

                    if (configView.SelectedNode.Tag is ApplicationNode)
                    {
                        contextMenuStrip.Items.Add(new ToolStripMenuItem("Create Protocol", null, OnCreateProtocolMenuClick));
                    }

                    if (configView.SelectedNode.Tag is RepositoryNode repository)
                    {
                        switch (repository.Type)
                        {
                            case EntryType.Protocol:
                                contextMenuStrip.Items.Add(new ToolStripMenuItem("Add", null, new ToolStripItem[]
                                {
                                    new ToolStripMenuItem("New Template", null, OnNewTemplateMenuClick),
                                    new ToolStripMenuItem("New Root Template", null, OnCreateRootTemplateMenuClick),
                                    new ToolStripMenuItem("New Folder", null, OnNewFolderMenuClick)
                                }));
                                contextMenuStrip.Items.Add(new ToolStripSeparator());

                                contextMenuStrip.Items.Add(new ToolStripMenuItem("Apply protocol", null, OnApplyProtocolMenuClick) { Enabled = canApplyProtocol });
                                break;

                            case EntryType.Folder:
                                contextMenuStrip.Items.Add(new ToolStripMenuItem("Add", null, new ToolStripItem[]
                                {
                                    new ToolStripMenuItem("New Template", null, OnNewTemplateMenuClick),
                                    new ToolStripMenuItem("New Root Template", null, OnCreateRootTemplateMenuClick),
                                    new ToolStripMenuItem("New Folder", null, OnNewFolderMenuClick),
                                }));
                                break;

                            case EntryType.Template:
                                contextMenuStrip.Items.Add(new ToolStripMenuItem("Open", null, OnOpenTemplateMenuClick));
                                break;
                        }
                    }

                    contextMenuStrip.Items.Add(new ToolStripSeparator());
                    contextMenuStrip.Items.Add(new ToolStripMenuItem("Rename", null, OnRenameNodeMenuClick));
                    contextMenuStrip.Items.Add(new ToolStripMenuItem("Delete", null, OnDeleteNodeMenuClick));
                }

                contextMenuStrip.Show(Cursor.Position);
            }
        }

        #region TreeView Context menu handlers

        private void OnCreateApplicationMenuClick(object sender, EventArgs e)
        {
            var newApp = new ApplicationNode();
            var newNode = new TreeNode("New Application", 0, 0) { Tag = newApp };

            _configManager.Config.Applications.Add(newApp);
            configView.Nodes.Add(newNode);
            configView.SelectedNode = newNode;

            configView.LabelEdit = true;
            configView.SelectedNode.BeginEdit();
        }

        private void OnCreateProtocolMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null && configView.SelectedNode.Tag is ApplicationNode appNode)
            {
                var newProtocol = new RepositoryNode(EntryType.Protocol);
                var newNode = new TreeNode("New Protocol", 0, 0) { Tag = newProtocol };

                appNode.Protocols.Add(newProtocol);
                configView.SelectedNode.Nodes.Add(newNode);
                configView.SelectedNode = newNode;

                configView.LabelEdit = true;
                configView.SelectedNode.BeginEdit();
            }
        }

        private void OnCreateRootTemplateMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null && configView.SelectedNode.Tag is RepositoryNode repository && repository.CanHaveFiles())
            {
                var name = Prompt.ShowDialog("New root template name:", "New Root Template");

                if (name != "")
                {
                    var template =
$@"namespace {string.Join(".", repository.PathTo().ToArray())}
{{
    [RootPacket]
    public class {name} : IRootTemplate
    {{
        
    }}
}}
";

                    var newItem = repository.AddItem(EntryType.Template, name, template);
                    var newNode = new TreeNode(name, 1, 1) { Tag = newItem };

                    configView.SelectedNode.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;

                    _configManager.Save();

                    CreateTemplateEditorTab(newItem);
                }
            }
        }

        private void OnNewTemplateMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null && configView.SelectedNode.Tag is RepositoryNode repository)
            {
                var name = Prompt.ShowDialog($"New template name:", "New Template");

                if (name != "")
                {
                    var template =
$@"namespace {string.Join(".", repository.PathTo().ToArray())}
{{
    public class {name}
    {{
        
    }}
}}
";

                    var newItem = repository.AddItem(EntryType.Template, name, template);
                    var newNode = new TreeNode(name, 1, 1) { Tag = newItem };

                    configView.SelectedNode.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;

                    _configManager.Save();

                    CreateTemplateEditorTab(newItem);
                }
            }
        }

        private void OnNewFolderMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null && configView.SelectedNode.Tag is RepositoryNode repository && repository.CanHaveFiles())
            {
                var newProtocol = repository.AddItem(EntryType.Folder);
                var newNode = new TreeNode("New Folder", 0, 0) { Tag = newProtocol };

                configView.SelectedNode.Nodes.Add(newNode);
                configView.SelectedNode = newNode;

                configView.LabelEdit = true;
                configView.SelectedNode.BeginEdit();
            }
        }

        private void OnOpenTemplateMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null)
            {
                if (configView.SelectedNode.Tag is RepositoryNode repository && repository.Content != null)
                {
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        if (tabPage.Tag is RepositoryNode repositoryNode)
                        {
                            if (Equals(repositoryNode, repository))
                            {
                                tabControl.SelectedTab = tabPage;
                                return;
                            }
                        }
                    }

                    CreateTemplateEditorTab(repository);
                }
            }
        }

        private void OnRenameNodeMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null)
            {
                configView.LabelEdit = true;
                configView.SelectedNode.BeginEdit();

                /*
                var name = "";

                if (configNode is ApplicationNode)
                {
                    name = Prompt.ShowDialog("New application name:", $"Rename application \"{configNode.Name}\"");
                }

                if (configNode is IPNode)
                {
                    name = Prompt.ShowDialog("New IP name:", $"Rename IP \"{configNode.Name}\"");
                }

                if (configNode is ProtocolNode)
                {
                    name = Prompt.ShowDialog("New protocol name:", $"Rename protocol \"{configNode.Name}\"");
                }

                if (configNode is TemplateNode)
                {
                    name = Prompt.ShowDialog("New template name:", $"Rename template \"{configNode.Name}\"");
                }

                if (name != "")
                {
                    configNode.Name = name;

                    _configManager.Save();
                }*/
            }
        }

        private void OnDeleteNodeMenuClick(object sender, EventArgs e)
        {
            // TODO: handle opened template editor tab if any

            if (configView.SelectedNode != null)
            {
                if (configView.SelectedNode.Parent == null)
                {
                    if (configView.SelectedNode.Tag is ApplicationNode appNode)
                    {
                        _configManager.Config.Applications.Remove(appNode);
                    }
                }
                else
                {
                    var parentNode = configView.SelectedNode.Parent;

                    if (parentNode.Tag is ApplicationNode parentAppNode)
                    {
                        if (configView.SelectedNode.Tag is IPNode ipNode)
                        {
                            parentAppNode.IPs.Remove(ipNode);
                        }

                        if (configView.SelectedNode.Tag is RepositoryNode repositoryNode)
                        {
                            parentAppNode.Protocols.Remove(repositoryNode);
                        }
                    }

                    if (parentNode.Tag is RepositoryNode parentRepositoryNode)
                    {
                        if (configView.SelectedNode.Tag is RepositoryNode repositoryNode)
                        {
                            parentRepositoryNode.Items.Remove(repositoryNode);
                        }
                    }
                }

                configView.SelectedNode.Remove();

                _configManager.Save();
            }
        }

        private void OnApplyProtocolMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode != null && configView.SelectedNode.Tag is RepositoryNode repositoryNode)
            {
                if (tabControl.SelectedTab.Controls[0] is TcpReconstructedInfoControl control)
                {
                    control.ApplyProtocol(repositoryNode);
                }
            }
        }

        #endregion
    }
}

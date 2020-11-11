using _010Proxy.Network;
using _010Proxy.Types;
using _010Proxy.Utils;
using _010Proxy.Views;
using SharpPcap;
using System;
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

        /*
        private void CreateCatalogNodes(TreeNode parentNode, List<ConfigCatalogNode> catalog)
        {
            foreach (var catalogItem in catalog)
            {
                var newNode = new TreeNode(catalogItem.Name, 0, 0)
                {
                    Tag = catalogItem
                };

                if (catalogItem.ChildCatalogNodes.Count > 0)
                {
                    CreateCatalogNodes(newNode, catalogItem.ChildCatalogNodes);
                }

                foreach (var ipNode in catalogItem.ChildIPNodes)
                {
                    newNode.Nodes.Add(new TreeNode(ipNode.Name));
                }

                if (!catalogItem.NodeOpened)
                {
                    newNode.Collapse();
                }

                parentNode.Nodes.Add(newNode);
            }
        }
         */

        private void LoadConfig()
        {
            configView.Nodes.Clear();

            var config = _configManager.Config;

            foreach (var app in config.Applications)
            {
                var newAppNode = new TreeNode(app.Name, 0, 0)
                {
                    Tag = app
                };

                foreach (var protocol in app.Protocols)
                {
                    var newProtocolNode = new TreeNode(protocol.Name, 0, 0)
                    {
                        Tag = protocol
                    };

                    foreach (var template in protocol.Templates)
                    {
                        var newTemplateNode = new TreeNode(template.Value.Name, 1, 1)
                        {
                            Tag = template.Value
                        };

                        newProtocolNode.Nodes.Add(newTemplateNode);
                    }

                    if (protocol.NodeOpened)
                    {
                        newProtocolNode.Expand();
                    }

                    newAppNode.Nodes.Add(newProtocolNode);
                }

                if (app.NodeOpened)
                {
                    newAppNode.Expand();
                }

                configView.Nodes.Add(newAppNode);
            }
        }

        public void SaveConfig()
        {
            _configManager.Save();
        }

        #region Tabs creation

        public void CreateWelcomeTab()
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

        private void CreateTemplateEditorTab(TemplateNode template)
        {
            var newTab = new TabPage($"{template.Name}");
            var newControl = new TemplateEditorControl()
            {
                Dock = DockStyle.Fill,
                ParentForm = this,
                ParentTab = newTab
            };

            newTab.Controls.Add(newControl);
            newTab.Tag = template;

            tabControl.TabPages.Add(newTab);
            tabControl.SelectTab(newTab);

            newControl.LoadTemplate(template);
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
                        // TODO: show prompt to notify user tab with this connection info already exists and whether he wants to still open new tab
                        tabControl.SelectedTab = tabPage;
                        return;
                    }
                }
            }

            CreateConnectionFlowTab(connectionInfo);
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

        #endregion

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            // ((ProxyTabControl)e.TabPage.Controls[0]).OnHide();
        }

        private void CreateConfigContextMenu(object sender, MouseEventArgs e)
        {
            configView.SelectedNode = configView.GetNodeAt(e.X, e.Y);

            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Items.Clear();

                if (configView.SelectedNode is null)
                {
                    contextMenuStrip.Items.Add(new ToolStripMenuItem("Create Application", null, OnCreateApplicationMenuClick));
                }
                else
                {
                    if (configView.SelectedNode.Tag is ApplicationNode)
                    {
                        contextMenuStrip.Items.Add(new ToolStripMenuItem("Create Protocol", null, OnCreateProtocolMenuClick));
                    }

                    if (configView.SelectedNode.Tag is ProtocolNode protocolNode)
                    {
                        contextMenuStrip.Items.Add(new ToolStripMenuItem("Apply protocol", null, OnApplyProtocolMenuClick)
                        {
                            Enabled = tabControl.SelectedTab.Controls[0] is TcpReconstructedInfoControl,
                            Tag = protocolNode
                        });

                        contextMenuStrip.Items.Add(new ToolStripMenuItem("New root template", null, OnCreateRootTemplateMenuClick));
                        contextMenuStrip.Items.Add(new ToolStripMenuItem("New template", null, OnNewTemplateMenuClick));
                    }

                    if (configView.SelectedNode.Tag is TemplateNode)
                    {
                        // contextMenuStrip.Items.Add(new ToolStripMenuItem("Rename", null, OnRenameTemplateMenuClick));
                        contextMenuStrip.Items.Add(new ToolStripMenuItem("Edit template", null, OnEditTemplateMenuClick));
                    }

                    contextMenuStrip.Items.Add(new ToolStripMenuItem("Delete", null, OnDeleteNodeMenuClick));
                }

                contextMenuStrip.Show(Cursor.Position);
            }
        }

        #region TreeView Context menu handlers

        private void OnCreateApplicationMenuClick(object sender, EventArgs e)
        {
            if (configView.SelectedNode is null)
            {
                var name = Prompt.ShowDialog("New application:", "New Application");

                if (name != "")
                {
                    var newApp = new ApplicationNode() { Name = name };
                    var newNode = new TreeNode(name, 0, 0) { Tag = newApp };

                    _configManager.Config.Applications.Add(newApp);
                    configView.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;
                }
            }
        }

        private void OnCreateProtocolMenuClick(object sender, EventArgs e)
        {
            if (!(configView.SelectedNode is null) && configView.SelectedNode.Tag is ApplicationNode appNode)
            {
                var name = Prompt.ShowDialog("New protocol:", "New Protocol");

                if (name != "")
                {
                    var newProtocol = new ProtocolNode() { Name = name };
                    var newNode = new TreeNode(name, 0, 0) { Tag = newProtocol };

                    appNode.Protocols.Add(newProtocol);
                    configView.SelectedNode.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;
                }
            }
        }

        private void OnCreateRootTemplateMenuClick(object sender, EventArgs e)
        {
            if (!(configView.SelectedNode is null) && configView.SelectedNode.Tag is ProtocolNode protocolNode)
            {
                var name = Prompt.ShowDialog($"New root template name for {protocolNode.Name}:", "New Root Template");

                if (name != "")
                {
                    var template = new TemplateNode()
                    {
                        Name = name,
                        Code =
$@"namespace {protocolNode.Name} {{
    [RootPacket]
    public class {name} : IRootTemplate
    {{
        
    }}
}}
"
                    };

                    var newNode = new TreeNode(name, 1, 1) { Tag = template };

                    protocolNode.Templates.Add(name, template);
                    configView.SelectedNode.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;

                    CreateTemplateEditorTab(template);
                }
            }
        }

        private void OnNewTemplateMenuClick(object sender, EventArgs e)
        {
            if (!(configView.SelectedNode is null) && configView.SelectedNode.Tag is ProtocolNode protocolNode)
            {
                var name = Prompt.ShowDialog($"New template name for {protocolNode.Name}:", "New Template");

                if (name != "")
                {
                    var template = new TemplateNode()
                    {
                        Name = name,
                        Code =
$@"namespace {protocolNode.Name} {{
    public class {name}
    {{
        
    }}
}}
"
                    };
                    var newNode = new TreeNode(name, 1, 1) { Tag = template };

                    protocolNode.Templates.Add(name, template);
                    configView.SelectedNode.Nodes.Add(newNode);
                    configView.SelectedNode = newNode;

                    CreateTemplateEditorTab(template);
                }
            }
        }

        private void OnEditTemplateMenuClick(object sender, EventArgs e)
        {
            if (!(configView.SelectedNode is null))
            {
                if (configView.SelectedNode.Tag is TemplateNode templateNode)
                {
                    CreateTemplateEditorTab(templateNode);
                }
            }
        }

        private void OnDeleteRootTemplateMenuClick(object sender, EventArgs e)
        {
            if (!(configView.SelectedNode is null))
            {
                if (configView.SelectedNode.Tag is ProtocolNode protocolNode)
                {
                    // protocolNode.RootTemplate = new TemplateNode();
                }
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
                        if (configView.SelectedNode.Tag is ProtocolNode protocolNode)
                        {
                            parentAppNode.Protocols.Remove(protocolNode);
                        }
                    }

                    if (parentNode.Tag is ProtocolNode parentProtocolNode)
                    {
                        if (configView.SelectedNode.Tag is TemplateNode templateNode)
                        {
                            parentProtocolNode.Templates.Remove(templateNode.EventType);
                        }
                    }
                }

                configView.SelectedNode.Remove();
            }
        }

        private void OnApplyProtocolMenuClick(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var protocol = (ProtocolNode)menuItem.Tag;

            if (tabControl.SelectedTab.Controls[0] is TcpReconstructedInfoControl control)
            {
                control.ApplyProtocol(protocol);
            }
        }

        #endregion

        private void configView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            configView.SelectedNode = configView.GetNodeAt(e.X, e.Y);
        }

        private void configView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            OnEditTemplateMenuClick(sender, e);
        }

        private void configView_AfterExpandOrCollapse(object sender, TreeViewEventArgs e)
        {
            if (configView.SelectedNode is null)
            {
                return;
            }

            if (configView.SelectedNode.Tag is ApplicationNode appNode)
            {
                appNode.NodeOpened = e.Node.IsExpanded;
            }

            if (configView.SelectedNode.Tag is ProtocolNode protocolNode)
            {
                protocolNode.NodeOpened = e.Node.IsExpanded;
            }
        }
    }
}

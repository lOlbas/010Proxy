using _010Proxy.Types;

namespace _010Proxy.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noFilesFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.captureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshInterfacesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.protocolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyTemplateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitter = new System.Windows.Forms.Splitter();
            this.configGroupBox = new System.Windows.Forms.GroupBox();
            this.configView = new System.Windows.Forms.TreeView();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.flowDataPreviewGroupBox = new System.Windows.Forms.GroupBox();
            this.flowDataView = new System.Windows.Forms.TreeView();
            this.leftPanelSplitter = new System.Windows.Forms.Splitter();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu.SuspendLayout();
            this.configGroupBox.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.flowDataPreviewGroupBox.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(350, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1146, 939);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Deselecting);
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.captureMenuItem,
            this.protocolMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1496, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.openRecentFileMenuItem,
            this.closeFileMenuItem,
            this.toolStripSeparator1,
            this.saveFileMenuItem,
            this.saveAsFileMenuItem,
            this.toolStripSeparator2,
            this.quitFileMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuItem.Text = "File";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openFileMenuItem.Text = "Open";
            this.openFileMenuItem.Click += new System.EventHandler(this.openFileMenuItem_Click);
            // 
            // openRecentFileMenuItem
            // 
            this.openRecentFileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noFilesFoundToolStripMenuItem});
            this.openRecentFileMenuItem.Name = "openRecentFileMenuItem";
            this.openRecentFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openRecentFileMenuItem.Text = "Open Recent";
            // 
            // noFilesFoundToolStripMenuItem
            // 
            this.noFilesFoundToolStripMenuItem.Enabled = false;
            this.noFilesFoundToolStripMenuItem.Name = "noFilesFoundToolStripMenuItem";
            this.noFilesFoundToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.noFilesFoundToolStripMenuItem.Text = "No files found";
            // 
            // closeFileMenuItem
            // 
            this.closeFileMenuItem.Enabled = false;
            this.closeFileMenuItem.Name = "closeFileMenuItem";
            this.closeFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.closeFileMenuItem.Text = "Close";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // saveFileMenuItem
            // 
            this.saveFileMenuItem.Enabled = false;
            this.saveFileMenuItem.Name = "saveFileMenuItem";
            this.saveFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveFileMenuItem.Text = "Save";
            // 
            // saveAsFileMenuItem
            // 
            this.saveAsFileMenuItem.Enabled = false;
            this.saveAsFileMenuItem.Name = "saveAsFileMenuItem";
            this.saveAsFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.saveAsFileMenuItem.Text = "Save As...";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // quitFileMenuItem
            // 
            this.quitFileMenuItem.Name = "quitFileMenuItem";
            this.quitFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitFileMenuItem.Size = new System.Drawing.Size(146, 22);
            this.quitFileMenuItem.Text = "Quit";
            this.quitFileMenuItem.Click += new System.EventHandler(this.quitFileMenuItem_Click);
            // 
            // captureMenuItem
            // 
            this.captureMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startCaptureMenuItem,
            this.stopCaptureMenuItem,
            this.restartCaptureMenuItem,
            this.refreshInterfacesMenuItem});
            this.captureMenuItem.Name = "captureMenuItem";
            this.captureMenuItem.Size = new System.Drawing.Size(61, 20);
            this.captureMenuItem.Text = "Capture";
            // 
            // startCaptureMenuItem
            // 
            this.startCaptureMenuItem.Enabled = false;
            this.startCaptureMenuItem.Name = "startCaptureMenuItem";
            this.startCaptureMenuItem.Size = new System.Drawing.Size(186, 22);
            this.startCaptureMenuItem.Text = "Start";
            // 
            // stopCaptureMenuItem
            // 
            this.stopCaptureMenuItem.Enabled = false;
            this.stopCaptureMenuItem.Name = "stopCaptureMenuItem";
            this.stopCaptureMenuItem.Size = new System.Drawing.Size(186, 22);
            this.stopCaptureMenuItem.Text = "Stop";
            // 
            // restartCaptureMenuItem
            // 
            this.restartCaptureMenuItem.Enabled = false;
            this.restartCaptureMenuItem.Name = "restartCaptureMenuItem";
            this.restartCaptureMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.restartCaptureMenuItem.Size = new System.Drawing.Size(186, 22);
            this.restartCaptureMenuItem.Text = "Restart";
            // 
            // refreshInterfacesMenuItem
            // 
            this.refreshInterfacesMenuItem.Enabled = false;
            this.refreshInterfacesMenuItem.Name = "refreshInterfacesMenuItem";
            this.refreshInterfacesMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshInterfacesMenuItem.Size = new System.Drawing.Size(186, 22);
            this.refreshInterfacesMenuItem.Text = "Refresh interfaces";
            // 
            // protocolMenuItem
            // 
            this.protocolMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applyTemplateMenuItem});
            this.protocolMenuItem.Name = "protocolMenuItem";
            this.protocolMenuItem.Size = new System.Drawing.Size(64, 20);
            this.protocolMenuItem.Text = "Protocol";
            // 
            // applyTemplateMenuItem
            // 
            this.applyTemplateMenuItem.Enabled = false;
            this.applyTemplateMenuItem.Name = "applyTemplateMenuItem";
            this.applyTemplateMenuItem.Size = new System.Drawing.Size(155, 22);
            this.applyTemplateMenuItem.Text = "Apply template";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // configImageList
            // 
            this.configImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.configImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.configImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(350, 24);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 939);
            this.splitter.TabIndex = 3;
            this.splitter.TabStop = false;
            // 
            // configGroupBox
            // 
            this.configGroupBox.Controls.Add(this.configView);
            this.configGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.configGroupBox.Location = new System.Drawing.Point(0, 0);
            this.configGroupBox.Name = "configGroupBox";
            this.configGroupBox.Size = new System.Drawing.Size(350, 600);
            this.configGroupBox.TabIndex = 4;
            this.configGroupBox.TabStop = false;
            this.configGroupBox.Text = "Configuration";
            // 
            // configView
            // 
            this.configView.AllowDrop = true;
            this.configView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configView.ImageIndex = 0;
            this.configView.ImageList = this.configImageList;
            this.configView.Location = new System.Drawing.Point(3, 16);
            this.configView.Name = "configView";
            this.configView.SelectedImageIndex = 0;
            this.configView.Size = new System.Drawing.Size(344, 581);
            this.configView.Sorted = true;
            this.configView.TabIndex = 2;
            this.configView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.configView_AfterLabelEdit);
            this.configView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.configView_AfterExpandOrCollapse);
            this.configView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.configView_AfterExpandOrCollapse);
            this.configView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.configView_ItemDrag);
            this.configView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.configView_NodeMouseClick);
            this.configView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.configView_NodeMouseDoubleClick);
            this.configView.DragDrop += new System.Windows.Forms.DragEventHandler(this.configView_DragDrop);
            this.configView.DragEnter += new System.Windows.Forms.DragEventHandler(this.configView_DragEnter);
            this.configView.DragOver += new System.Windows.Forms.DragEventHandler(this.configView_DragOver);
            this.configView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.configView_KeyDown);
            this.configView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CreateConfigContextMenu);
            this.configView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateConfigContextMenu);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.flowDataPreviewGroupBox);
            this.leftPanel.Controls.Add(this.leftPanelSplitter);
            this.leftPanel.Controls.Add(this.configGroupBox);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 24);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(350, 939);
            this.leftPanel.TabIndex = 4;
            // 
            // flowDataPreviewGroupBox
            // 
            this.flowDataPreviewGroupBox.Controls.Add(this.flowDataView);
            this.flowDataPreviewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowDataPreviewGroupBox.Location = new System.Drawing.Point(0, 603);
            this.flowDataPreviewGroupBox.Name = "flowDataPreviewGroupBox";
            this.flowDataPreviewGroupBox.Size = new System.Drawing.Size(350, 336);
            this.flowDataPreviewGroupBox.TabIndex = 6;
            this.flowDataPreviewGroupBox.TabStop = false;
            this.flowDataPreviewGroupBox.Text = "Flow Data Preview";
            // 
            // flowDataView
            // 
            this.flowDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowDataView.Indent = 23;
            this.flowDataView.ItemHeight = 20;
            this.flowDataView.Location = new System.Drawing.Point(3, 16);
            this.flowDataView.Name = "flowDataView";
            this.flowDataView.Size = new System.Drawing.Size(344, 317);
            this.flowDataView.TabIndex = 0;
            this.flowDataView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.flowDataView_NodeMouseClick);
            // 
            // leftPanelSplitter
            // 
            this.leftPanelSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.leftPanelSplitter.Location = new System.Drawing.Point(0, 600);
            this.leftPanelSplitter.Name = "leftPanelSplitter";
            this.leftPanelSplitter.Size = new System.Drawing.Size(350, 3);
            this.leftPanelSplitter.TabIndex = 5;
            this.leftPanelSplitter.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 963);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1496, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "Whaaaat";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1496, 985);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.statusStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "010Proxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.configGroupBox.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.flowDataPreviewGroupBox.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.MenuStrip mainMenu;
        public System.Windows.Forms.ToolStripMenuItem restartCaptureMenuItem;
        public System.Windows.Forms.ToolStripMenuItem refreshInterfacesMenuItem;
        public System.Windows.Forms.ToolStripMenuItem stopCaptureMenuItem;
        public System.Windows.Forms.ToolStripMenuItem startCaptureMenuItem;
        public System.Windows.Forms.ToolStripMenuItem captureMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.TreeView configView;
        private System.Windows.Forms.Splitter splitter;
        private System.Windows.Forms.ImageList configImageList;
        private System.Windows.Forms.ToolStripMenuItem protocolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyTemplateMenuItem;
        private System.Windows.Forms.GroupBox configGroupBox;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Splitter leftPanelSplitter;
        private System.Windows.Forms.GroupBox flowDataPreviewGroupBox;
        private System.Windows.Forms.TreeView flowDataView;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRecentFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noFilesFoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsFileMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem quitFileMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}


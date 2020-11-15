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
            this.captureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartCaptureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshInterfacesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.protocolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyTemplateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.configView = new System.Windows.Forms.TreeView();
            this.configImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitter = new System.Windows.Forms.Splitter();
            this.configGroupBox = new System.Windows.Forms.GroupBox();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.leftPanelSplitter = new System.Windows.Forms.Splitter();
            this.flowDataPreviewGroupBox = new System.Windows.Forms.GroupBox();
            this.flowDataView = new System.Windows.Forms.TreeView();
            this.mainMenu.SuspendLayout();
            this.configGroupBox.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.flowDataPreviewGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(200, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(881, 612);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Deselecting);
            this.tabControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabControl_KeyDown);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureMenuItem,
            this.protocolMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1081, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
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
            // configView
            // 
            this.configView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configView.ImageIndex = 0;
            this.configView.ImageList = this.configImageList;
            this.configView.Location = new System.Drawing.Point(3, 16);
            this.configView.Name = "configView";
            this.configView.SelectedImageIndex = 0;
            this.configView.Size = new System.Drawing.Size(194, 281);
            this.configView.TabIndex = 2;
            this.configView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.configView_AfterExpandOrCollapse);
            this.configView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.configView_AfterExpandOrCollapse);
            this.configView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.configView_NodeMouseClick);
            this.configView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.configView_NodeMouseDoubleClick);
            this.configView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CreateConfigContextMenu);
            this.configView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateConfigContextMenu);
            // 
            // configImageList
            // 
            this.configImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.configImageList.ImageSize = new System.Drawing.Size(20, 20);
            this.configImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(200, 24);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 612);
            this.splitter.TabIndex = 3;
            this.splitter.TabStop = false;
            // 
            // configGroupBox
            // 
            this.configGroupBox.Controls.Add(this.configView);
            this.configGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.configGroupBox.Location = new System.Drawing.Point(0, 0);
            this.configGroupBox.Name = "configGroupBox";
            this.configGroupBox.Size = new System.Drawing.Size(200, 300);
            this.configGroupBox.TabIndex = 4;
            this.configGroupBox.TabStop = false;
            this.configGroupBox.Text = "Configuration";
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.flowDataPreviewGroupBox);
            this.leftPanel.Controls.Add(this.leftPanelSplitter);
            this.leftPanel.Controls.Add(this.configGroupBox);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 24);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(200, 612);
            this.leftPanel.TabIndex = 4;
            // 
            // leftPanelSplitter
            // 
            this.leftPanelSplitter.Dock = System.Windows.Forms.DockStyle.Top;
            this.leftPanelSplitter.Location = new System.Drawing.Point(0, 300);
            this.leftPanelSplitter.Name = "leftPanelSplitter";
            this.leftPanelSplitter.Size = new System.Drawing.Size(200, 3);
            this.leftPanelSplitter.TabIndex = 5;
            this.leftPanelSplitter.TabStop = false;
            // 
            // flowDataPreviewGroupBox
            // 
            this.flowDataPreviewGroupBox.Controls.Add(this.flowDataView);
            this.flowDataPreviewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowDataPreviewGroupBox.Location = new System.Drawing.Point(0, 303);
            this.flowDataPreviewGroupBox.Name = "flowDataPreviewGroupBox";
            this.flowDataPreviewGroupBox.Size = new System.Drawing.Size(200, 309);
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
            this.flowDataView.Size = new System.Drawing.Size(194, 290);
            this.flowDataView.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 636);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.mainMenu);
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
    }
}


namespace _010Proxy.Views
{
    partial class TcpConnectionInfoControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.packetsTable = new System.Windows.Forms.DataGridView();
            this.directionColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.indexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ackColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seqColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter = new System.Windows.Forms.Splitter();
            this.packetPreview = new Be.Windows.Forms.HexBox();
            this.actionsPanel = new System.Windows.Forms.Panel();
            this.ignoreFilterListCheckBox = new System.Windows.Forms.CheckBox();
            this.toggleOpCodesButton = new System.Windows.Forms.Button();
            this.protocolsList = new System.Windows.Forms.ComboBox();
            this.selectProtocolLabel = new System.Windows.Forms.Label();
            this.applicationsList = new System.Windows.Forms.ComboBox();
            this.applyProtocolButton = new System.Windows.Forms.Button();
            this.switchViewButton = new System.Windows.Forms.Button();
            this.eventsTable = new System.Windows.Forms.DataGridView();
            this.eventDrectionColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.eventIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventDataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tablesPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.packetsTable)).BeginInit();
            this.actionsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventsTable)).BeginInit();
            this.tablesPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // packetsTable
            // 
            this.packetsTable.AllowUserToAddRows = false;
            this.packetsTable.AllowUserToDeleteRows = false;
            this.packetsTable.AllowUserToResizeRows = false;
            this.packetsTable.BackgroundColor = System.Drawing.Color.White;
            this.packetsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.packetsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.directionColumn,
            this.indexColumn,
            this.timeColumn,
            this.ackColumn,
            this.seqColumn,
            this.dataLengthColumn,
            this.dataColumn});
            this.packetsTable.Location = new System.Drawing.Point(0, 0);
            this.packetsTable.MultiSelect = false;
            this.packetsTable.Name = "packetsTable";
            this.packetsTable.ReadOnly = true;
            this.packetsTable.RowHeadersVisible = false;
            this.packetsTable.RowHeadersWidth = 25;
            this.packetsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.packetsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.packetsTable.Size = new System.Drawing.Size(334, 212);
            this.packetsTable.TabIndex = 0;
            this.packetsTable.Scroll += new System.Windows.Forms.ScrollEventHandler(this.packetsTable_Scroll);
            this.packetsTable.SelectionChanged += new System.EventHandler(this.packetsTable_SelectionChanged);
            this.packetsTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.packetsTable_MouseDown);
            // 
            // directionColumn
            // 
            this.directionColumn.HeaderText = "";
            this.directionColumn.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.directionColumn.MinimumWidth = 25;
            this.directionColumn.Name = "directionColumn";
            this.directionColumn.ReadOnly = true;
            this.directionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.directionColumn.Width = 25;
            // 
            // indexColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.indexColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.indexColumn.HeaderText = "No.";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            this.indexColumn.Width = 45;
            // 
            // timeColumn
            // 
            this.timeColumn.HeaderText = "Time";
            this.timeColumn.Name = "timeColumn";
            this.timeColumn.ReadOnly = true;
            // 
            // ackColumn
            // 
            this.ackColumn.HeaderText = "Ack";
            this.ackColumn.Name = "ackColumn";
            this.ackColumn.ReadOnly = true;
            this.ackColumn.ToolTipText = "Acknowledgment number";
            // 
            // seqColumn
            // 
            this.seqColumn.HeaderText = "Seq";
            this.seqColumn.Name = "seqColumn";
            this.seqColumn.ReadOnly = true;
            this.seqColumn.ToolTipText = "Sequence number";
            // 
            // dataLengthColumn
            // 
            this.dataLengthColumn.HeaderText = "Length";
            this.dataLengthColumn.Name = "dataLengthColumn";
            this.dataLengthColumn.ReadOnly = true;
            // 
            // dataColumn
            // 
            this.dataColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataColumn.HeaderText = "Data";
            this.dataColumn.Name = "dataColumn";
            this.dataColumn.ReadOnly = true;
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter.Location = new System.Drawing.Point(0, 281);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(768, 3);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // packetPreview
            // 
            this.packetPreview.ColumnInfoVisible = true;
            this.packetPreview.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.packetPreview.Font = new System.Drawing.Font("Courier New", 10F);
            this.packetPreview.LineInfoVisible = true;
            this.packetPreview.Location = new System.Drawing.Point(0, 284);
            this.packetPreview.Name = "packetPreview";
            this.packetPreview.ReadOnly = true;
            this.packetPreview.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.packetPreview.Size = new System.Drawing.Size(768, 228);
            this.packetPreview.StringViewVisible = true;
            this.packetPreview.TabIndex = 2;
            this.packetPreview.UseFixedBytesPerLine = true;
            this.packetPreview.VScrollBarVisible = true;
            // 
            // actionsPanel
            // 
            this.actionsPanel.Controls.Add(this.ignoreFilterListCheckBox);
            this.actionsPanel.Controls.Add(this.toggleOpCodesButton);
            this.actionsPanel.Controls.Add(this.protocolsList);
            this.actionsPanel.Controls.Add(this.selectProtocolLabel);
            this.actionsPanel.Controls.Add(this.applicationsList);
            this.actionsPanel.Controls.Add(this.applyProtocolButton);
            this.actionsPanel.Controls.Add(this.switchViewButton);
            this.actionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.actionsPanel.Location = new System.Drawing.Point(0, 0);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Size = new System.Drawing.Size(768, 31);
            this.actionsPanel.TabIndex = 3;
            // 
            // ignoreFilterListCheckBox
            // 
            this.ignoreFilterListCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreFilterListCheckBox.AutoSize = true;
            this.ignoreFilterListCheckBox.Location = new System.Drawing.Point(409, 7);
            this.ignoreFilterListCheckBox.Name = "ignoreFilterListCheckBox";
            this.ignoreFilterListCheckBox.Size = new System.Drawing.Size(81, 17);
            this.ignoreFilterListCheckBox.TabIndex = 6;
            this.ignoreFilterListCheckBox.Text = "Ignore Filter";
            this.ignoreFilterListCheckBox.UseVisualStyleBackColor = true;
            this.ignoreFilterListCheckBox.CheckedChanged += new System.EventHandler(this.ignoreFilterListCheckBox_CheckedChanged);
            // 
            // toggleOpCodesButton
            // 
            this.toggleOpCodesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleOpCodesButton.Enabled = false;
            this.toggleOpCodesButton.Location = new System.Drawing.Point(495, 4);
            this.toggleOpCodesButton.Name = "toggleOpCodesButton";
            this.toggleOpCodesButton.Size = new System.Drawing.Size(95, 23);
            this.toggleOpCodesButton.TabIndex = 5;
            this.toggleOpCodesButton.Text = "Toggle OpCodes";
            this.toggleOpCodesButton.UseVisualStyleBackColor = true;
            this.toggleOpCodesButton.Click += new System.EventHandler(this.toggleOpCodesButton_Click);
            // 
            // protocolsList
            // 
            this.protocolsList.Enabled = false;
            this.protocolsList.FormattingEnabled = true;
            this.protocolsList.Location = new System.Drawing.Point(213, 5);
            this.protocolsList.Name = "protocolsList";
            this.protocolsList.Size = new System.Drawing.Size(121, 21);
            this.protocolsList.TabIndex = 3;
            // 
            // selectProtocolLabel
            // 
            this.selectProtocolLabel.AutoSize = true;
            this.selectProtocolLabel.Location = new System.Drawing.Point(4, 9);
            this.selectProtocolLabel.Name = "selectProtocolLabel";
            this.selectProtocolLabel.Size = new System.Drawing.Size(81, 13);
            this.selectProtocolLabel.TabIndex = 2;
            this.selectProtocolLabel.Text = "Select protocol:";
            // 
            // applicationsList
            // 
            this.applicationsList.Enabled = false;
            this.applicationsList.FormattingEnabled = true;
            this.applicationsList.Location = new System.Drawing.Point(88, 5);
            this.applicationsList.Name = "applicationsList";
            this.applicationsList.Size = new System.Drawing.Size(121, 21);
            this.applicationsList.TabIndex = 1;
            // 
            // applyProtocolButton
            // 
            this.applyProtocolButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyProtocolButton.Enabled = false;
            this.applyProtocolButton.Location = new System.Drawing.Point(594, 4);
            this.applyProtocolButton.Name = "applyProtocolButton";
            this.applyProtocolButton.Size = new System.Drawing.Size(83, 23);
            this.applyProtocolButton.TabIndex = 4;
            this.applyProtocolButton.Text = "Apply Protocol";
            this.applyProtocolButton.UseVisualStyleBackColor = true;
            this.applyProtocolButton.Click += new System.EventHandler(this.applyProtocolButton_Click);
            // 
            // switchViewButton
            // 
            this.switchViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.switchViewButton.Enabled = false;
            this.switchViewButton.Location = new System.Drawing.Point(681, 4);
            this.switchViewButton.Name = "switchViewButton";
            this.switchViewButton.Size = new System.Drawing.Size(83, 23);
            this.switchViewButton.TabIndex = 0;
            this.switchViewButton.Text = "View Events";
            this.switchViewButton.UseVisualStyleBackColor = true;
            this.switchViewButton.Click += new System.EventHandler(this.switchViewButton_Click);
            // 
            // eventsTable
            // 
            this.eventsTable.AllowUserToAddRows = false;
            this.eventsTable.AllowUserToDeleteRows = false;
            this.eventsTable.AllowUserToResizeRows = false;
            this.eventsTable.BackgroundColor = System.Drawing.Color.White;
            this.eventsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eventDrectionColumn,
            this.eventIndexColumn,
            this.eventTimeColumn,
            this.eventTypeColumn,
            this.eventLengthColumn,
            this.eventDataColumn});
            this.eventsTable.Location = new System.Drawing.Point(401, 0);
            this.eventsTable.MultiSelect = false;
            this.eventsTable.Name = "eventsTable";
            this.eventsTable.ReadOnly = true;
            this.eventsTable.RowHeadersVisible = false;
            this.eventsTable.RowHeadersWidth = 25;
            this.eventsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.eventsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.eventsTable.Size = new System.Drawing.Size(367, 212);
            this.eventsTable.TabIndex = 4;
            this.eventsTable.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.eventsTable_CellMouseClick);
            this.eventsTable.Scroll += new System.Windows.Forms.ScrollEventHandler(this.eventsTable_Scroll);
            this.eventsTable.SelectionChanged += new System.EventHandler(this.eventsTable_SelectionChanged);
            // 
            // eventDrectionColumn
            // 
            this.eventDrectionColumn.HeaderText = "";
            this.eventDrectionColumn.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.eventDrectionColumn.MinimumWidth = 25;
            this.eventDrectionColumn.Name = "eventDrectionColumn";
            this.eventDrectionColumn.ReadOnly = true;
            this.eventDrectionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.eventDrectionColumn.Width = 25;
            // 
            // eventIndexColumn
            // 
            this.eventIndexColumn.HeaderText = "No.";
            this.eventIndexColumn.Name = "eventIndexColumn";
            this.eventIndexColumn.ReadOnly = true;
            this.eventIndexColumn.Width = 45;
            // 
            // eventTimeColumn
            // 
            this.eventTimeColumn.HeaderText = "Time";
            this.eventTimeColumn.Name = "eventTimeColumn";
            this.eventTimeColumn.ReadOnly = true;
            // 
            // eventTypeColumn
            // 
            this.eventTypeColumn.HeaderText = "Op Code";
            this.eventTypeColumn.Name = "eventTypeColumn";
            this.eventTypeColumn.ReadOnly = true;
            this.eventTypeColumn.Width = 250;
            // 
            // eventLengthColumn
            // 
            this.eventLengthColumn.HeaderText = "Length";
            this.eventLengthColumn.Name = "eventLengthColumn";
            this.eventLengthColumn.ReadOnly = true;
            // 
            // eventDataColumn
            // 
            this.eventDataColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.eventDataColumn.HeaderText = "Data";
            this.eventDataColumn.Name = "eventDataColumn";
            this.eventDataColumn.ReadOnly = true;
            // 
            // tablesPanel
            // 
            this.tablesPanel.Controls.Add(this.eventsTable);
            this.tablesPanel.Controls.Add(this.packetsTable);
            this.tablesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesPanel.Location = new System.Drawing.Point(0, 31);
            this.tablesPanel.Name = "tablesPanel";
            this.tablesPanel.Size = new System.Drawing.Size(768, 253);
            this.tablesPanel.TabIndex = 5;
            // 
            // TcpConnectionInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.tablesPanel);
            this.Controls.Add(this.packetPreview);
            this.Controls.Add(this.actionsPanel);
            this.Name = "TcpConnectionInfoControl";
            this.Size = new System.Drawing.Size(768, 512);
            ((System.ComponentModel.ISupportInitialize)(this.packetsTable)).EndInit();
            this.actionsPanel.ResumeLayout(false);
            this.actionsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventsTable)).EndInit();
            this.tablesPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView packetsTable;
        private System.Windows.Forms.Splitter splitter;
        private Be.Windows.Forms.HexBox packetPreview;
        private System.Windows.Forms.DataGridViewImageColumn directionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ackColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seqColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumn;
        private System.Windows.Forms.Panel actionsPanel;
        private System.Windows.Forms.Button switchViewButton;
        private System.Windows.Forms.Label selectProtocolLabel;
        private System.Windows.Forms.ComboBox applicationsList;
        private System.Windows.Forms.ComboBox protocolsList;
        private System.Windows.Forms.Button applyProtocolButton;
        private System.Windows.Forms.DataGridView eventsTable;
        private System.Windows.Forms.Panel tablesPanel;
        private System.Windows.Forms.DataGridViewImageColumn eventDrectionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventDataColumn;
        private System.Windows.Forms.Button toggleOpCodesButton;
        private System.Windows.Forms.CheckBox ignoreFilterListCheckBox;
    }
}

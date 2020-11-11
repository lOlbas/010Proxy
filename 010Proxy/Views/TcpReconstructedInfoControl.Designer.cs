namespace _010Proxy.Views
{
    partial class TcpReconstructedInfoControl
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
            this.packetsTable = new System.Windows.Forms.DataGridView();
            this.directionColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.indexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.firstPacketTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastPacketTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalPacketsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.packetPreview = new Be.Windows.Forms.HexBox();
            ((System.ComponentModel.ISupportInitialize)(this.packetsTable)).BeginInit();
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
            this.firstPacketTimeColumn,
            this.lastPacketTimeColumn,
            this.totalPacketsColumn,
            this.lengthColumn,
            this.dataColumn});
            this.packetsTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.packetsTable.Location = new System.Drawing.Point(0, 0);
            this.packetsTable.MultiSelect = false;
            this.packetsTable.Name = "packetsTable";
            this.packetsTable.ReadOnly = true;
            this.packetsTable.RowHeadersVisible = false;
            this.packetsTable.RowHeadersWidth = 25;
            this.packetsTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.packetsTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.packetsTable.Size = new System.Drawing.Size(768, 320);
            this.packetsTable.TabIndex = 0;
            this.packetsTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.packetsTable_CellDoubleClick);
            this.packetsTable.Scroll += new System.Windows.Forms.ScrollEventHandler(this.packetsTable_Scroll);
            this.packetsTable.SelectionChanged += new System.EventHandler(this.packetsTable_SelectionChanged);
            // 
            // directionColumn
            // 
            this.directionColumn.HeaderText = "";
            this.directionColumn.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.directionColumn.MinimumWidth = 25;
            this.directionColumn.Name = "directionColumn";
            this.directionColumn.ReadOnly = true;
            this.directionColumn.Width = 25;
            // 
            // indexColumn
            // 
            this.indexColumn.HeaderText = "No.";
            this.indexColumn.Name = "indexColumn";
            this.indexColumn.ReadOnly = true;
            this.indexColumn.Width = 45;
            // 
            // firstPacketTimeColumn
            // 
            this.firstPacketTimeColumn.HeaderText = "First Packet Time";
            this.firstPacketTimeColumn.Name = "firstPacketTimeColumn";
            this.firstPacketTimeColumn.ReadOnly = true;
            this.firstPacketTimeColumn.Width = 120;
            // 
            // lastPacketTimeColumn
            // 
            this.lastPacketTimeColumn.HeaderText = "Last Packet Time";
            this.lastPacketTimeColumn.Name = "lastPacketTimeColumn";
            this.lastPacketTimeColumn.ReadOnly = true;
            this.lastPacketTimeColumn.Width = 120;
            // 
            // totalPacketsColumn
            // 
            this.totalPacketsColumn.HeaderText = "Total Packets";
            this.totalPacketsColumn.Name = "totalPacketsColumn";
            this.totalPacketsColumn.ReadOnly = true;
            // 
            // lengthColumn
            // 
            this.lengthColumn.HeaderText = "Length";
            this.lengthColumn.Name = "lengthColumn";
            this.lengthColumn.ReadOnly = true;
            // 
            // dataColumn
            // 
            this.dataColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataColumn.HeaderText = "Data";
            this.dataColumn.Name = "dataColumn";
            this.dataColumn.ReadOnly = true;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 320);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(768, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // packetPreview
            // 
            this.packetPreview.ColumnInfoVisible = true;
            this.packetPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetPreview.Font = new System.Drawing.Font("Courier New", 10F);
            this.packetPreview.LineInfoVisible = true;
            this.packetPreview.Location = new System.Drawing.Point(0, 323);
            this.packetPreview.Name = "packetPreview";
            this.packetPreview.ReadOnly = true;
            this.packetPreview.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.packetPreview.Size = new System.Drawing.Size(768, 189);
            this.packetPreview.StringViewVisible = true;
            this.packetPreview.TabIndex = 2;
            this.packetPreview.UseFixedBytesPerLine = true;
            this.packetPreview.VScrollBarVisible = true;
            // 
            // TcpReconstructedInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.packetPreview);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.packetsTable);
            this.Name = "TcpReconstructedInfoControl";
            this.Size = new System.Drawing.Size(768, 512);
            ((System.ComponentModel.ISupportInitialize)(this.packetsTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView packetsTable;
        private System.Windows.Forms.DataGridViewImageColumn directionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstPacketTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastPacketTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalPacketsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumn;
        private System.Windows.Forms.Splitter splitter1;
        private Be.Windows.Forms.HexBox packetPreview;
    }
}

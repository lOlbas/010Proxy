namespace _010Proxy.Views
{
    partial class CaptureTrafficControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.appNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocolColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalDataColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.appNameColumn,
            this.clientColumn,
            this.serverColumn,
            this.protocolColumn,
            this.totalDataColumn,
            this.infoColumn});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(768, 512);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
            // 
            // appNameColumn
            // 
            this.appNameColumn.DataPropertyName = "AppName";
            this.appNameColumn.HeaderText = "App Name";
            this.appNameColumn.Name = "appNameColumn";
            this.appNameColumn.ReadOnly = true;
            // 
            // clientColumn
            // 
            this.clientColumn.DataPropertyName = "LocalEndPoint";
            this.clientColumn.HeaderText = "Client";
            this.clientColumn.Name = "clientColumn";
            this.clientColumn.ReadOnly = true;
            this.clientColumn.Width = 150;
            // 
            // serverColumn
            // 
            this.serverColumn.DataPropertyName = "RemoteEndPoint";
            this.serverColumn.HeaderText = "Server";
            this.serverColumn.Name = "serverColumn";
            this.serverColumn.ReadOnly = true;
            this.serverColumn.Width = 150;
            // 
            // protocolColumn
            // 
            this.protocolColumn.DataPropertyName = "ProtocolName";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.protocolColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.protocolColumn.HeaderText = "Protocol";
            this.protocolColumn.Name = "protocolColumn";
            this.protocolColumn.ReadOnly = true;
            this.protocolColumn.Width = 75;
            // 
            // totalDataColumn
            // 
            this.totalDataColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.totalDataColumn.DataPropertyName = "TotalData";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalDataColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.totalDataColumn.HeaderText = "Total Data";
            this.totalDataColumn.Name = "totalDataColumn";
            this.totalDataColumn.ReadOnly = true;
            this.totalDataColumn.Width = 80;
            // 
            // infoColumn
            // 
            this.infoColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.infoColumn.HeaderText = "Info";
            this.infoColumn.Name = "infoColumn";
            this.infoColumn.ReadOnly = true;
            // 
            // CaptureTrafficControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView);
            this.Name = "CaptureTrafficControl";
            this.Size = new System.Drawing.Size(768, 512);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn appNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalDataColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn infoColumn;
    }
}

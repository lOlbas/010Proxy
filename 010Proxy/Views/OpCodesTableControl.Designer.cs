namespace _010Proxy.Views
{
    partial class OpCodesTableControl
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
            this.opCodesTable = new System.Windows.Forms.DataGridView();
            this.filter = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.opcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.opCodesTable)).BeginInit();
            this.SuspendLayout();
            // 
            // opCodesTable
            // 
            this.opCodesTable.AllowUserToAddRows = false;
            this.opCodesTable.AllowUserToDeleteRows = false;
            this.opCodesTable.AllowUserToResizeRows = false;
            this.opCodesTable.BackgroundColor = System.Drawing.Color.White;
            this.opCodesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.opCodesTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.filter,
            this.id,
            this.opcode});
            this.opCodesTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.opCodesTable.Location = new System.Drawing.Point(0, 0);
            this.opCodesTable.Name = "opCodesTable";
            this.opCodesTable.RowHeadersWidth = 4;
            this.opCodesTable.Size = new System.Drawing.Size(480, 640);
            this.opCodesTable.TabIndex = 0;
            // 
            // filter
            // 
            this.filter.HeaderText = "";
            this.filter.Name = "filter";
            this.filter.Width = 30;
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.Width = 70;
            // 
            // opcode
            // 
            this.opcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.opcode.HeaderText = "Op Code";
            this.opcode.Name = "opcode";
            // 
            // OpCodesTableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.opCodesTable);
            this.Name = "OpCodesTableControl";
            this.Size = new System.Drawing.Size(480, 640);
            ((System.ComponentModel.ISupportInitialize)(this.opCodesTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView opCodesTable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn filter;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn opcode;
    }
}

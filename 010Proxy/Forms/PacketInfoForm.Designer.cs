namespace _010Proxy.Forms
{
    partial class PacketInfoForm
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
            this.templateEditor = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // templateEditor
            // 
            this.templateEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateEditor.Lexer = ScintillaNET.Lexer.Cpp;
            this.templateEditor.Location = new System.Drawing.Point(0, 0);
            this.templateEditor.Name = "templateEditor";
            this.templateEditor.Size = new System.Drawing.Size(800, 450);
            this.templateEditor.TabIndex = 0;
            this.templateEditor.CharAdded += new System.EventHandler<ScintillaNET.CharAddedEventArgs>(this.scriptInput_CharAdded);
            this.templateEditor.InsertCheck += new System.EventHandler<ScintillaNET.InsertCheckEventArgs>(this.scriptInput_InsertCheck);
            this.templateEditor.TextChanged += new System.EventHandler(this.scriptInput_TextChanged);
            // 
            // PacketInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.templateEditor);
            this.Name = "PacketInfoForm";
            this.Text = "PacketInfoForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla templateEditor;
    }
}
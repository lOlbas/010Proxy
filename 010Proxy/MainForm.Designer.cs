namespace _010Proxy
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
            this.captureTrafficControl = new _010Proxy.Views.CaptureTrafficControl();
            this.homeControl = new _010Proxy.Views.HomeControl();
            this.SuspendLayout();
            // 
            // captureTrafficControl
            // 
            this.captureTrafficControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.captureTrafficControl.Location = new System.Drawing.Point(0, 0);
            this.captureTrafficControl.Name = "captureTrafficControl";
            this.captureTrafficControl.Size = new System.Drawing.Size(800, 450);
            this.captureTrafficControl.TabIndex = 1;
            // 
            // homeControl
            // 
            this.homeControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homeControl.Location = new System.Drawing.Point(0, 0);
            this.homeControl.Name = "homeControl";
            this.homeControl.ParentForm = null;
            this.homeControl.Size = new System.Drawing.Size(800, 450);
            this.homeControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.captureTrafficControl);
            this.Controls.Add(this.homeControl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "010Proxy";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Views.HomeControl homeControl;
        private Views.CaptureTrafficControl captureTrafficControl;
    }
}


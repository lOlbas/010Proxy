namespace _010Proxy.Views
{
    partial class HomeControl
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
            this.captureTrafficButton = new System.Windows.Forms.Button();
            this.startProxyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // captureTrafficButton
            // 
            this.captureTrafficButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.captureTrafficButton.Location = new System.Drawing.Point(320, 201);
            this.captureTrafficButton.Name = "captureTrafficButton";
            this.captureTrafficButton.Size = new System.Drawing.Size(128, 23);
            this.captureTrafficButton.TabIndex = 0;
            this.captureTrafficButton.Text = "Capture Traffic";
            this.captureTrafficButton.UseVisualStyleBackColor = true;
            this.captureTrafficButton.Click += new System.EventHandler(this.captureTrafficButton_Click);
            // 
            // startProxyButton
            // 
            this.startProxyButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startProxyButton.Location = new System.Drawing.Point(320, 257);
            this.startProxyButton.Name = "startProxyButton";
            this.startProxyButton.Size = new System.Drawing.Size(128, 23);
            this.startProxyButton.TabIndex = 1;
            this.startProxyButton.Text = "Start Proxy Servers";
            this.startProxyButton.UseVisualStyleBackColor = true;
            // 
            // HomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startProxyButton);
            this.Controls.Add(this.captureTrafficButton);
            this.Name = "HomeControl";
            this.Size = new System.Drawing.Size(768, 512);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button captureTrafficButton;
        private System.Windows.Forms.Button startProxyButton;
    }
}

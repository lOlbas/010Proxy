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
            this.availableDevicesListView = new System.Windows.Forms.ListView();
            this.noInterfacesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // captureTrafficButton
            // 
            this.captureTrafficButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.captureTrafficButton.Location = new System.Drawing.Point(320, 133);
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
            this.startProxyButton.Location = new System.Drawing.Point(320, 189);
            this.startProxyButton.Name = "startProxyButton";
            this.startProxyButton.Size = new System.Drawing.Size(128, 23);
            this.startProxyButton.TabIndex = 1;
            this.startProxyButton.Text = "Start Proxy Servers";
            this.startProxyButton.UseVisualStyleBackColor = true;
            // 
            // availableDevicesListView
            // 
            this.availableDevicesListView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.availableDevicesListView.FullRowSelect = true;
            this.availableDevicesListView.HideSelection = false;
            this.availableDevicesListView.Location = new System.Drawing.Point(209, 256);
            this.availableDevicesListView.MultiSelect = false;
            this.availableDevicesListView.Name = "availableDevicesListView";
            this.availableDevicesListView.Size = new System.Drawing.Size(350, 97);
            this.availableDevicesListView.TabIndex = 3;
            this.availableDevicesListView.UseCompatibleStateImageBehavior = false;
            this.availableDevicesListView.View = System.Windows.Forms.View.List;
            this.availableDevicesListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.availableDevicesListView_ItemSelectionChanged);
            this.availableDevicesListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.availableDevicesListView_MouseDoubleClick);
            // 
            // noInterfacesLabel
            // 
            this.noInterfacesLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noInterfacesLabel.Location = new System.Drawing.Point(209, 256);
            this.noInterfacesLabel.Name = "noInterfacesLabel";
            this.noInterfacesLabel.Size = new System.Drawing.Size(350, 13);
            this.noInterfacesLabel.TabIndex = 4;
            this.noInterfacesLabel.Text = "No interfaces found.";
            this.noInterfacesLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.noInterfacesLabel.Visible = false;
            // 
            // HomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.noInterfacesLabel);
            this.Controls.Add(this.availableDevicesListView);
            this.Controls.Add(this.startProxyButton);
            this.Controls.Add(this.captureTrafficButton);
            this.Name = "HomeControl";
            this.Size = new System.Drawing.Size(768, 512);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button captureTrafficButton;
        private System.Windows.Forms.Button startProxyButton;
        private System.Windows.Forms.ListView availableDevicesListView;
        private System.Windows.Forms.Label noInterfacesLabel;
    }
}

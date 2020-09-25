using System;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public partial class HomeControl : UserControl
    {
        public new MainForm ParentForm { get; set; }

        public HomeControl()
        {
            InitializeComponent();
        }

        private void captureTrafficButton_Click(object sender, EventArgs e)
        {
            ParentForm?.ShowCaptureTrafficView();
        }
    }
}

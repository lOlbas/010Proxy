using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _010Proxy.Views;

namespace _010Proxy
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            homeControl.ParentForm = this;
            captureTrafficControl.ParentForm = this;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            homeControl.Show();
            captureTrafficControl.Hide();
        }

        public void ShowCaptureTrafficView()
        {
            homeControl.Hide();
            captureTrafficControl.Show();
        }
    }
}

using _010Proxy.Utils;
using SharpPcap;
using System;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public partial class HomeControl : ProxyTabControl
    {
        public HomeControl()
        {
            InitializeComponent();
        }

        private void RefreshInterfaces()
        {
            availableDevicesListView.Items.Clear();

            if (ParentForm.Sniffer.Devices.Count > 0)
            {
                foreach (var device in ParentForm.Sniffer.Devices)
                {
                    var friendlyName = device.GetFriendlyName();

                    if (friendlyName != null)
                    {
                        availableDevicesListView.Items.Add(new ListViewItem(friendlyName)
                        {
                            Tag = device
                        });
                    }
                }

                availableDevicesListView.Visible = true;
                noInterfacesLabel.Visible = false;
            }
            else
            {
                availableDevicesListView.Visible = false;
                noInterfacesLabel.Visible = true;
            }
        }

        private void StartSelectedNetworkAnalysis()
        {
            if (availableDevicesListView.SelectedIndices.Count > 0)
            {
                var device = availableDevicesListView.SelectedItems[0].Tag as ICaptureDevice;
                ParentForm?.StartNetworkAnalysis(device);
            }
        }

        private void captureTrafficButton_Click(object sender, EventArgs e)
        {
            StartSelectedNetworkAnalysis();
        }

        private void availableDevicesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var itemSelected = availableDevicesListView.SelectedIndices.Count > 0;

            ParentForm.startCaptureMenuItem.Enabled = itemSelected;
            captureTrafficButton.Enabled = itemSelected;
        }

        private void availableDevicesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var info = availableDevicesListView.HitTest(e.X, e.Y);
            var item = info.Item;

            if (item != null)
            {
                StartSelectedNetworkAnalysis();
            }
            else
            {
                // this.listView1.SelectedItems.Clear();
            }
        }

        public override void OnShow()
        {
            ParentForm.refreshInterfacesMenuItem.Click += RefreshInterfacesCb;
            ParentForm.startCaptureMenuItem.Click += captureTrafficButton_Click;
            captureTrafficButton.Enabled = false;
            RefreshInterfaces();
            UpdateMenu(true);
        }

        public override void OnHide()
        {
            ParentForm.refreshInterfacesMenuItem.Click -= RefreshInterfacesCb;
            UpdateMenu(false);
        }

        public override void OnClose()
        {
            UpdateMenu(false);
        }

        private void RefreshInterfacesCb(object sender, EventArgs e)
        {
            RefreshInterfaces();
        }

        protected override void UpdateMenu(bool show)
        {
            ParentForm.refreshInterfacesMenuItem.Enabled = show;
        }
    }
}

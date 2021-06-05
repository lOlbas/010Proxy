using _010Proxy.Network;
using _010Proxy.Utils;
using _010Proxy.Utils.Extensions;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using SharpPcap.LibPcap;

namespace _010Proxy.Views
{
    public partial class CaptureTrafficControl : ProxyTabControl
    {
        private ICaptureDevice _device;

        private List<ConnectionInfo> _connections;
        private BindingSource _connectionsBinding;

        // TODO: connection ignore list

        public CaptureTrafficControl()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView, new object[] { true });
        }

        public void StartNetworkAnalysis(ICaptureDevice device)
        {
            _device = device;
            _connections = new List<ConnectionInfo>();
            _connectionsBinding = new BindingSource(new BindingList<ConnectionInfo>(_connections), null);
            dataGridView.DataSource = _connectionsBinding;

            ParentForm.Sniffer.OnPacketReceive += OnPacketReceive;
            ParentForm.Sniffer.StartCapture(_device);
            UpdateMenu();
        }

        public void StopNetworkAnalysis()
        {
            ParentForm.Sniffer.OnPacketReceive -= OnPacketReceive;
            ParentForm.Sniffer.StopCapture();
            UpdateMenu();
        }

        private void OnPacketReceive(Packet packet, RawCapture rawPacket)
        {
            if (packet.Is<IPPacket>(out var ipPacket))
            {
                HandleIpPacket(ipPacket, rawPacket);
            }
        }

        private void HandleIpPacket(IPPacket ipPacket, RawCapture rawPacket)
        {
            if (ipPacket.Is<TcpPacket>(out var tcpPacket))
            {
                HandleTcpPacket(tcpPacket, ipPacket, rawPacket);
            }

            // TODO: UDP is not yet supported
            if (ipPacket.Is<UdpPacket>(out var udpPacket))
            {
                // HandleUdpPacket(udpPacket, ipPacket, timeVal);
            }
        }

        private void HandleTcpPacket(TcpPacket tcpPacket, IPPacket ipPacket, RawCapture rawPacket)
        {
            var srcIp = ipPacket.SourceAddress;
            var dstIp = ipPacket.DestinationAddress;

            var localIp = "";
            var serverIp = "";

            var srcIsLocal = false;
            var dstIsLocal = false;

            var type = typeof(TcpPacket);
            var protocolName = "TCP";

            var sender = SenderEnum.Unknown;

            IPEndPoint localEndPoint = null, remoteEndPoint = null;

            if (srcIp.IsLocal())
            {
                localIp = srcIp.ToString();
                srcIsLocal = true;
                localEndPoint = new IPEndPoint(srcIp, tcpPacket.SourcePort);
                remoteEndPoint = new IPEndPoint(dstIp, tcpPacket.DestinationPort);
                sender = SenderEnum.Client;
            }
            else
            {
                serverIp = srcIp.ToString();
            }

            if (dstIp.IsLocal())
            {
                localIp = dstIp.ToString();
                dstIsLocal = true;
                localEndPoint = new IPEndPoint(dstIp, tcpPacket.DestinationPort);
                remoteEndPoint = new IPEndPoint(srcIp, tcpPacket.SourcePort);
                sender = SenderEnum.Server;
            }
            else
            {
                serverIp = dstIp.ToString();
            }

            if (srcIsLocal && dstIsLocal)
            {
                // TODO: throw error since we can't determine who is client and who is server without user intervention
            }

            switch (remoteEndPoint.Port)
            {
                case 80:
                    protocolName = "HTTP";
                    break;

                case 443:
                    protocolName = "HTTPS";
                    break;
            }

            if (localEndPoint != null)
            {
                var connectionInfo = _connections.FirstOrDefault(ci => ci.EqualsTo(protocolName, localEndPoint, remoteEndPoint));

                if (connectionInfo == null)
                {
                    connectionInfo = new ConnectionInfo("Unassigned", protocolName, localEndPoint, remoteEndPoint, _device);
                    _connections.Add(connectionInfo);

                    RefreshConnectionsTable();
                }

                connectionInfo.AddPacket(new TimedPacket(tcpPacket, rawPacket, sender));
            }
        }

        private void HandleUdpPacket(UdpPacket udpPacket, IPPacket ipPacket)
        {

        }

        private void RefreshConnectionsTable()
        {
            dataGridView.Invoke((MethodInvoker)delegate
            {
                var (rowIndex, colIndex) = (dataGridView.FirstDisplayedScrollingRowIndex, dataGridView.FirstDisplayedScrollingColumnIndex);
                _connectionsBinding.ResetBindings(true);

                if (rowIndex >= 0)
                {
                    dataGridView.FirstDisplayedScrollingRowIndex = rowIndex;
                    dataGridView.FirstDisplayedScrollingColumnIndex = colIndex;
                }
            });
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ParentForm?.ShowConnectionPackets(_connections[e.RowIndex]);
            }
        }

        private void stopCaptureMenuItem_Click(object sender, EventArgs e)
        {
            StopNetworkAnalysis();
        }

        public override void OnShow()
        {
            ParentForm.stopCaptureMenuItem.Click += stopCaptureMenuItem_Click;
            UpdateMenu(true);
        }

        public override void OnHide()
        {
            UpdateMenu(false);
        }

        public override void OnClose()
        {
            StopNetworkAnalysis();
            UpdateMenu(false);
        }

        protected override void UpdateMenu(bool show = true)
        {
            var deviceStarted = _device != null && _device.Started;

            ParentForm.startCaptureMenuItem.Enabled = show && !deviceStarted;
            ParentForm.stopCaptureMenuItem.Enabled = show && deviceStarted;
            ParentForm.restartCaptureMenuItem.Enabled = show && deviceStarted;
        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ParentForm.contextMenuStrip.Items.Clear();
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("View raw packets", null, OnShowRawPacketsMenuClick));
                ParentForm.contextMenuStrip.Items.Add(new ToolStripSeparator());
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("Export as PCAP", null, OnExportAsPCAPMenuClick));
                ParentForm.contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void OnShowRawPacketsMenuClick(object sender, EventArgs e)
        {
            ParentForm?.ShowConnectionPackets(_connections[dataGridView.SelectedRows[0].Index]);
        }

        private void OnExportAsPCAPMenuClick(object sender, EventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var filename = dialog.FileName;

                    if (!filename.EndsWith(".pcap"))
                    {
                        filename += ".pcap";
                    }

                    var writer = new CaptureFileWriterDevice(filename);

                    foreach (var packet in _connections[dataGridView.SelectedRows[0].Index].Packets)
                    {
                        writer.Write(packet.RawPacket);
                    }

                    writer.Close();
                }
            }
            
        }
    }
}

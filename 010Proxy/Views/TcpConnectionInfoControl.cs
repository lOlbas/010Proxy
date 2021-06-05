using _010Proxy.Network;
using _010Proxy.Templates;
using _010Proxy.Templates.Parsers;
using _010Proxy.Types;
using _010Proxy.Utils;
using Be.Windows.Forms;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using _010Proxy.Forms;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.Npcap;

namespace _010Proxy.Views
{
    public partial class TcpConnectionInfoControl : ProxyTabControl
    {
        private ConnectionInfo _connectionInfo;
        private ulong _packetIndex = 1;
        private ulong _eventIndex = 1;

        private TemplateParser _templateParser;

        private Bitmap _iconClientSource = Properties.Resources.IconClientSource;
        private Bitmap _iconServerSource = Properties.Resources.IconServerSource;

        private bool _stickPacketsToBottom = true;
        private bool _stickEventsToBottom = true;

        private Dictionary<Type, List<object>> _opCodeIgnoreList = new Dictionary<Type, List<object>>();

        public TcpConnectionInfoControl()
        {
            InitializeComponent();

            packetsTable.Visible = true;
            packetsTable.Dock = DockStyle.Fill;
            eventsTable.Visible = false;

            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, packetsTable, new object[] { true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, eventsTable, new object[] { true });
        }

        private void UpdateUI()
        {
            var applications = ParentForm.ConfigManager.Config.Applications;
            var hasApplications = false;
            var hasProtocols = false;

            foreach (var application in applications)
            {
                applicationsList.Items.Add(application.Name);
            }

            if (applications.Count > 0)
            {
                hasApplications = true;
                applicationsList.SelectedIndex = 0;

                foreach (var protocol in applications[0].Protocols)
                {
                    protocolsList.Items.Add(protocol.Name);
                }

                if (applications[0].Protocols.Count > 0)
                {
                    hasProtocols = true;
                    protocolsList.SelectedIndex = 0;
                }
            }

            applicationsList.Enabled = hasApplications;
            protocolsList.Enabled = hasProtocols;

            applyProtocolButton.Enabled = hasProtocols;
            switchViewButton.Enabled = _templateParser != null;
            toggleOpCodesButton.Enabled = _templateParser != null;
        }

        public void LoadConnection(ConnectionInfo connectionInfo)
        {
            UpdateUI();

            _connectionInfo = connectionInfo;
            packetsTable.Rows.Clear();

            foreach (var packet in _connectionInfo.Packets.ToList())
            {
                DisplayPacket(packet);
            }

            _connectionInfo.OnPacketArrive += OnPacketArrive;
            _connectionInfo.OnEventsParse += OnEventsParse;
        }

        private void DisplayPacket(TimedPacket timedPacket)
        {
            if (timedPacket.Packet.Is<TcpPacket>(out var tcpPacket))
            {
                packetsTable.Invoke((MethodInvoker)delegate
               {
                   var icon = timedPacket.Sender == SenderEnum.Client ? _iconClientSource : _iconServerSource;
                   var localTime = timedPacket.Time.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

                   var rowIndex = packetsTable.Rows.Add(icon, _packetIndex++, localTime, tcpPacket.AcknowledgmentNumber, tcpPacket.SequenceNumber, tcpPacket.PayloadData.Length, "");

                   packetsTable.Rows[rowIndex].Tag = timedPacket;

                   if (_stickPacketsToBottom)
                   {
                       packetsTable.FirstDisplayedScrollingRowIndex = rowIndex;
                   }
               });
            }
        }

        private void ReloadPackets()
        {
            _packetIndex = 1;
            packetsTable.Rows.Clear();

            foreach (var packet in _connectionInfo.Packets.ToList())
            {
                DisplayPacket(packet);
            }
        }

        private bool FilterEvent(ParsedEvent parsedEvent)
        {
            if (ignoreFilterListCheckBox.Checked)
            {
                return true;
            }

            foreach (var opCode in parsedEvent.OpCodes)
            {
                var opCodeType = opCode.Value.GetType();

                if (_opCodeIgnoreList.ContainsKey(opCodeType))
                {
                    if (_opCodeIgnoreList[opCodeType].Contains(opCode.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void DisplayEvent(ParsedEvent parsedEvent)
        {
            eventsTable.Invoke((MethodInvoker)delegate
           {
               var icon = parsedEvent.Sender == SenderEnum.Client ? _iconClientSource : _iconServerSource;
               var localTime = parsedEvent.Time.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

               var rowIndex = eventsTable.Rows.Add(icon, _eventIndex++, localTime, parsedEvent.OpCodes.FirstOrDefault().Value, parsedEvent.Length, "");

               eventsTable.Rows[rowIndex].Tag = parsedEvent;
               eventsTable.Rows[rowIndex].Visible = FilterEvent(parsedEvent);

               if (_stickEventsToBottom && eventsTable.Rows[rowIndex].Visible)
               {
                   eventsTable.FirstDisplayedScrollingRowIndex = eventsTable.RowCount - 1;
               }
           });
        }

        private void ReloadEvents()
        {
            foreach (DataGridViewRow row in eventsTable.Rows)
            {
                row.Visible = FilterEvent((ParsedEvent)row.Tag);
            }
        }

        private void OnPacketArrive(TimedPacket timedPacket)
        {
            DisplayPacket(timedPacket);
        }

        private void OnEventsParse(List<ParsedEvent> parsedEvents)
        {
            foreach (var parsedEvent in parsedEvents)
            {
                DisplayEvent(parsedEvent);
            }
        }

        // TODO: a racing condition occurs here when new packets are arrived while we are in the process of applying protocol,
        //  leading to new packet events appearing on top of the table
        public async void ApplyProtocol(RepositoryNode protocol)
        {
            eventsTable.Rows.Clear();
            _eventIndex = 1;

            await Task.Factory.StartNew(() => ApplyProtocolThreaded(protocol));

            if (_templateParser != null)
            {
                UpdateUI();
                ToggleEventsView();
            }
        }

        private void ApplyProtocolThreaded(RepositoryNode protocol)
        {
            try
            {
                _templateParser = new TemplateParser();
                _templateParser.LoadProtocol(protocol);

                if (_templateParser.ProtocolConfig != null)
                {
                    _opCodeIgnoreList = _templateParser.ProtocolConfig.OpCodesIgnoreList;
                }

                _connectionInfo.ParseEvents(_templateParser);
            }
            catch (Exception e)
            {
                _templateParser = null;
                MessageBox.Show(e.Message, "Error applying protocol", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void applyProtocolButton_Click(object sender, EventArgs e)
        {
            var protocol = ParentForm.ConfigManager.Config.Applications[applicationsList.SelectedIndex].Protocols[protocolsList.SelectedIndex];

            ApplyProtocol(protocol);
        }

        private void switchViewButton_Click(object sender, EventArgs e)
        {
            if (switchViewButton.Text == "View Events")
            {
                ToggleEventsView();
            }
            else if (switchViewButton.Text == "View Packets")
            {
                TogglePacketsView();
            }
        }

        private void ToggleEventsView()
        {
            switchViewButton.Text = "View Packets";

            eventsTable.Visible = true;
            eventsTable.Dock = DockStyle.Fill;
            packetsTable.Visible = false;
        }

        private void TogglePacketsView()
        {
            switchViewButton.Text = "View Events";

            packetsTable.Visible = true;
            packetsTable.Dock = DockStyle.Fill;
            eventsTable.Visible = false;
        }

        public void HighlightFieldPreview(FieldMeta fieldMeta)
        {
            packetPreview.Select(fieldMeta.Offset, fieldMeta.Length);
        }

        public void HighlightFieldPreview(long offset, long length)
        {
            packetPreview.Select(offset, length);
        }

        private void eventsTable_SelectionChanged(object sender, EventArgs e)
        {
            if (eventsTable.CurrentCell == null)
            {
                ParentForm.PreviewObject(null);
            }
            else if (eventsTable.Rows[eventsTable.CurrentCell.RowIndex].Tag is ParsedEvent parsedEvent)
            {
                PreviewEvent(parsedEvent);
            }
        }

        private void PreviewEvent(ParsedEvent parsedEvent)
        {
            var data = _connectionInfo.GetEventBytes(parsedEvent);

            packetPreview.Invoke((MethodInvoker)delegate { packetPreview.ByteProvider = new DynamicByteProvider(data); });

            if (parsedEvent.ParseMode == ParseMode.Root && _templateParser != null)
            {
                //parsedEvent.EventInstance = new TemplateReader(_templateParser, data, ParseMode.Complete).ReadTemplateEvent(parsedEvent);
                parsedEvent.ParseMode = ParseMode.Complete;
            }

            ParentForm.Invoke((MethodInvoker)delegate { ParentForm.PreviewObject(parsedEvent.EventInstance); });
        }

        private void packetsTable_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                _stickPacketsToBottom = packetsTable.Rows[packetsTable.Rows.Count - 1].Displayed;
            }
        }

        private void eventsTable_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                _stickEventsToBottom = eventsTable.Rows[eventsTable.Rows.Count - 1].Displayed;
            }
        }

        private void eventsTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ParentForm.contextMenuStrip.Items.Clear();
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("Copy OpCode name", null, OnCopyOpCodeMenuClick));
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("Export raw bytes", null, OnExportRawBytesMenuClick));
                ParentForm.contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void packetsTable_MouseDown(object sender, MouseEventArgs e)
        {
            var rowIndex = packetsTable.HitTest(e.X, e.Y).RowIndex;
            var colIndex = packetsTable.HitTest(e.X, e.Y).ColumnIndex;

            if (rowIndex == -1)
            {
                return;
            }

            packetsTable.ClearSelection();
            packetsTable.CurrentCell = packetsTable.Rows[rowIndex].Cells[colIndex];
            packetsTable.Rows[rowIndex].Selected = true;

            //var timedPacket = _connectionInfo.Packets[packetsTable.CurrentCell.RowIndex];
            var timedPacket = packetsTable.Rows[rowIndex].Tag as TimedPacket;

            if (timedPacket != null)
            {
                packetPreview.ByteProvider = new DynamicByteProvider(timedPacket.Packet.PayloadData);
            }
            else
            {
                Console.WriteLine("Null");
            }
            
            Console.WriteLine($"mousedown", rowIndex);
            if (e.Button == MouseButtons.Right)
            {
                ParentForm.contextMenuStrip.Items.Clear();
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("Remove packet", null, OnRemovePacketMenuClick));
                ParentForm.contextMenuStrip.Items.Add(new ToolStripMenuItem("Remove all packets below", null, OnRemoveAllPacketsBelowMenuClick));
                ParentForm.contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void packetsTable_SelectionChanged(object sender, EventArgs e)
        {
            if (packetsTable.CurrentCell == null)
            {
                return;
            }

            var timedPacket = packetsTable.Rows[packetsTable.CurrentCell.RowIndex].Tag as TimedPacket;

            if (timedPacket != null)
            {
                packetPreview.ByteProvider = new DynamicByteProvider(timedPacket.Packet.PayloadData);
            }
            else
            {
                Console.WriteLine("Null");
            }
        }

        private void OnRemovePacketMenuClick(object sender, EventArgs e)
        {
            _connectionInfo.Packets.Remove(packetsTable.Rows[packetsTable.CurrentCell.RowIndex].Tag as TimedPacket);
            packetsTable.Rows[packetsTable.CurrentCell.RowIndex].Visible = false;
        }

        private void OnRemoveAllPacketsBelowMenuClick(object sender, EventArgs e)
        {
            var packetIndex = _connectionInfo.Packets.IndexOf(packetsTable.Rows[packetsTable.CurrentCell.RowIndex].Tag as TimedPacket);
            _connectionInfo.Packets.RemoveRange(packetIndex, _connectionInfo.Packets.Count - packetIndex);
            ReloadPackets();
        }

        private void OnCopyOpCodeMenuClick(object sender, EventArgs e)
        {
            Clipboard.SetText(eventsTable.Rows[eventsTable.CurrentCell.RowIndex].Cells[3].Value.ToString());
        }

        private void OnExportRawBytesMenuClick(object sender, EventArgs e)
        {
            if (eventsTable.Rows[eventsTable.CurrentCell.RowIndex].Tag is ParsedEvent parsedEvent)
            {
                var stream = parsedEvent.Sender == SenderEnum.Client ? _connectionInfo.ClientStream : _connectionInfo.ServerStream;
                var pos = stream.Position;
                var data = new byte[parsedEvent.Length];

                stream.Seek(parsedEvent.Offset, SeekOrigin.Begin);
                stream.Read(data, 0, (int)parsedEvent.Length);

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Packets");
                File.WriteAllBytes(path + $"/{DateTimeOffset.Now.ToUnixTimeMilliseconds()}.txt", data);

                stream.Seek(pos, SeekOrigin.Begin);
            }
        }

        private void toggleOpCodesButton_Click(object sender, EventArgs e)
        {
            using (var opCodesTableForm = new OpCodesTableForm())
            {
                opCodesTableForm.DisplayOpCodes(_templateParser.OpCodesTypes, _opCodeIgnoreList);

                if (opCodesTableForm.ShowDialog() == DialogResult.OK)
                {
                    _opCodeIgnoreList = opCodesTableForm.GetIgnoreList();

                    ReloadEvents();
                }
            }
        }

        private void ignoreFilterListCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ReloadEvents();
        }
    }
}
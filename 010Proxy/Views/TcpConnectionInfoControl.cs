using _010Proxy.Network;
using _010Proxy.Templates.Parsers;
using _010Proxy.Types;
using _010Proxy.Utils;
using Be.Windows.Forms;
using PacketDotNet;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        }

        public void LoadConnection(ConnectionInfo connectionInfo)
        {
            UpdateUI();

            _connectionInfo = connectionInfo;

            foreach (var packet in _connectionInfo.Packets.ToList())
            {
                DisplayPacket(packet);
            }

            _connectionInfo.OnPacketArrive += OnPacketArrive;
        }

        private void DisplayPacket(TimedPacket timedPacket)
        {
            if (timedPacket.Packet.Is<TcpPacket>(out var tcpPacket))
            {
                packetsTable.Invoke((MethodInvoker)delegate
                {
                    var icon = timedPacket.Sender == SenderEnum.Client ? _iconClientSource : _iconServerSource;
                    var localTime = timedPacket.Time.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

                    packetsTable.Rows.Add(icon, _packetIndex++, localTime, tcpPacket.AcknowledgmentNumber, tcpPacket.SequenceNumber, tcpPacket.PayloadData.Length, "");

                    if (_stickPacketsToBottom)
                    {
                        packetsTable.FirstDisplayedScrollingRowIndex = packetsTable.RowCount - 1;
                    }
                });
            }
        }

        private void DisplayEvent(ParsedEvent parsedEvent)
        {
            eventsTable.Invoke((MethodInvoker)delegate
            {
                var icon = parsedEvent.Sender == SenderEnum.Client ? _iconClientSource : _iconServerSource;
                var localTime = parsedEvent.Time.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

                var rowIndex = eventsTable.Rows.Add(icon, _eventIndex++, localTime, parsedEvent.OpCode, parsedEvent.Length, "");

                eventsTable.Rows[rowIndex].Tag = parsedEvent;

                if (_stickEventsToBottom)
                {
                    eventsTable.FirstDisplayedScrollingRowIndex = eventsTable.RowCount - 1;
                }
            });
        }

        private void OnPacketArrive(TimedPacket timedPacket)
        {
            DisplayPacket(timedPacket);
        }

        private void packetsTable_SelectionChanged(object sender, System.EventArgs e)
        {
            var timedPacket = _connectionInfo.Packets[packetsTable.CurrentCell.RowIndex];

            packetPreview.ByteProvider = new DynamicByteProvider(timedPacket.Packet.PayloadData);
        }

        public async void ApplyProtocol(RepositoryNode protocol)
        {
            eventsTable.Rows.Clear();
            await Task.Factory.StartNew(() => ApplyProtocolThreaded(protocol));
            UpdateUI();
        }

        private void ApplyProtocolThreaded(RepositoryNode protocol)
        {
            _templateParser = new TemplateParser();
            _templateParser.LoadProtocol(protocol);
            // TODO: handle errors to show to user
            _connectionInfo.ParseEvents(_templateParser);

            foreach (var parsedEvent in _connectionInfo.ClientStream.ParsedEvents.Concat(_connectionInfo.ServerStream.ParsedEvents).OrderBy(pe => pe.Time))
            {
                DisplayEvent(parsedEvent);
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
                switchViewButton.Text = "View Packets";

                eventsTable.Visible = true;
                eventsTable.Dock = DockStyle.Fill;
                packetsTable.Visible = false;
            }
            else if (switchViewButton.Text == "View Packets")
            {
                switchViewButton.Text = "View Events";

                packetsTable.Visible = true;
                packetsTable.Dock = DockStyle.Fill;
                eventsTable.Visible = false;
            }
        }

        public void HighlightFieldPreview(FieldMeta fieldMeta)
        {
            packetPreview.Select(fieldMeta.Offset, fieldMeta.Length);
        }

        private void eventsTable_SelectionChanged(object sender, EventArgs e)
        {
            if (eventsTable.Rows[eventsTable.CurrentCell.RowIndex].Tag is ParsedEvent parsedEvent)
            {
                PreviewEvent(parsedEvent);
            }
        }

        private void PreviewEvent(ParsedEvent parsedEvent)
        {
            var data = _connectionInfo.GetEventBytes(parsedEvent);

            packetPreview.Invoke((MethodInvoker)delegate
            {
                packetPreview.ByteProvider = new DynamicByteProvider(data);
            });

            if (parsedEvent.ParseMode == ParseMode.Root && _templateParser != null)
            {
                parsedEvent.Data = _templateParser.ParseBuffer(data, ParseMode.Complete);
                parsedEvent.ParseMode = ParseMode.Complete;
            }

            ParentForm.Invoke((MethodInvoker)delegate
            {
                ParentForm.PreviewFlowData(parsedEvent.Data);
            });
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
    }
}

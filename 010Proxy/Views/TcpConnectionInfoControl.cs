using _010Proxy.Network;
using _010Proxy.Utils;
using Be.Windows.Forms;
using PacketDotNet;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    // TODO: somehow merge with TcpReconstructedInfoControl and switch between views on click
    public partial class TcpConnectionInfoControl : ProxyTabControl
    {
        private ConnectionInfo _connectionInfo;
        private ulong _packetIndex = 1;

        private Color _currentCellColor = Color.DeepSkyBlue;
        private Color _previousNumberColor = Color.Aqua;
        private Color _followingNumberColor = Color.LawnGreen;

        public TcpConnectionInfoControl()
        {
            InitializeComponent();
        }

        public void LoadPackets(ConnectionInfo connectionInfo)
        {
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
                    var icon = timedPacket.Sender == SenderEnum.Client ? Properties.Resources.IconClientSource : Properties.Resources.IconServerSource;
                    var localTime = timedPacket.Time.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

                    var rowIndex = packetsTable.Rows.Add(icon, _packetIndex++, localTime, tcpPacket.AcknowledgmentNumber, tcpPacket.SequenceNumber, tcpPacket.PayloadData.Length, "N/A");

                    packetsTable.Rows[rowIndex].Tag = timedPacket;
                });
            }
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

        private void packetsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            // TODO: clear coloring on ESC
            // TODO: highlight newly arrived packets
            switch (e.ColumnIndex)
            {
                case 3:
                case 4:
                    var timedPacket = (TimedPacket)packetsTable.Rows[e.RowIndex].Tag;
                    var ackNumber = (uint)packetsTable[3, e.RowIndex].Value;
                    var seqNumber = (uint)packetsTable[4, e.RowIndex].Value;
                    var senderFilter = timedPacket.Sender == SenderEnum.Client ? SenderEnum.Server : SenderEnum.Client;
                    var filterColIndex = e.ColumnIndex == 3 ? 4 : 3; // We highlight opposite number column

                    ResetPacketsColors();

                    // TODO: tweak logic, absolutely not perfect
                    HighlightPreviousPackets(senderFilter, filterColIndex, ackNumber, seqNumber, e.RowIndex);
                    HighlightFollowingPackets(senderFilter, filterColIndex, ackNumber, seqNumber, timedPacket.Packet.PayloadData.Length, e.RowIndex + 1);

                    break;
            }
        }

        private void ResetPacketsColors()
        {
            foreach (DataGridViewRow row in packetsTable.Rows)
            {
                row.Cells[3].Style.BackColor = Color.White;
                row.Cells[4].Style.BackColor = Color.White;
            }
        }

        private void HighlightPreviousPackets(SenderEnum senderFilter, int filterColIndex, uint ackNumber, uint seqNumber, int toRowIndex)
        {
            if (packetsTable.Rows.Count < toRowIndex)
            {
                return;
            }

            for (var i = 0; i < toRowIndex; i++)
            {
                var row = packetsTable.Rows[i];
                var timedPacket = (TimedPacket)row.Tag;
                var rowAckNumber = (uint)row.Cells[3].Value;
                var rowSeqNumber = (uint)row.Cells[4].Value;
                var rowDataLength = (int)row.Cells[5].Value;

                var isSynPacket = rowAckNumber == 0 && rowSeqNumber == ackNumber - 1;

                if (timedPacket.Sender == senderFilter && (rowAckNumber == seqNumber || isSynPacket))
                {
                    row.Cells[filterColIndex].Style.BackColor = _previousNumberColor;
                }
            }
        }

        private void HighlightFollowingPackets(SenderEnum senderFilter, int filterColIndex, uint ackNumber, uint seqNumber, int dataLength, int fromRowIndex)
        {
            var totalRows = packetsTable.Rows.Count;

            if (totalRows < fromRowIndex)
            {
                return;
            }

            for (var i = fromRowIndex; i < totalRows; i++)
            {
                var row = packetsTable.Rows[i];
                var timedPacket = (TimedPacket)row.Tag;
                var rowAckNumber = (uint)row.Cells[3].Value;
                var rowSeqNumber = (uint)row.Cells[4].Value;
                var rowDataLength = (int)row.Cells[5].Value;

                var isSynPacket = ackNumber == 0 && rowAckNumber == seqNumber + 1;
                var isExpectedAck = isSynPacket || (rowAckNumber >= seqNumber && rowAckNumber <= seqNumber + dataLength);
                var isExpectedSeq = (rowSeqNumber == ackNumber || isSynPacket);

                if (timedPacket.Sender == senderFilter && isExpectedAck && isExpectedSeq)
                {
                    row.Cells[filterColIndex].Style.BackColor = _followingNumberColor;
                }
            }
        }
    }
}

using _010Proxy.Network.TCP;
using _010Proxy.Utils.Extensions;
using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace _010Proxy.Network
{
    public class ConnectionInfo : INotifyPropertyChanged
    {
        public string AppName { get; }
        public string ProtocolName { get; }
        public IPEndPoint LocalEndPoint { get; }
        public IPEndPoint RemoteEndPoint { get; }

        public bool ServerDetected { get; }

        public List<TimedPacket> Packets { get; }
        public List<TcpFlow> Flows { get; }
        private Type _packetType;
        private ulong _totalData;

        public delegate void PacketReceivedHandler(TimedPacket timedPacket);
        public event PacketReceivedHandler OnPacketArrive;

        public delegate void NewFlowHandler(TcpFlow tcpFlow);
        public event NewFlowHandler OnNewFlow;

        public delegate void FlowUpdateHandler(TcpFlow tcpFlow);
        public event FlowUpdateHandler OnFlowUpdate;

        public ulong TotalData
        {
            get => _totalData;
            set
            {
                if (_totalData == value) return;
                _totalData = value;
                OnPropertyChanged("TotalData");
            }
        }

        public ConnectionInfo(string appName, string protocolName, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint, Type packetType)
        {
            AppName = appName;
            ProtocolName = protocolName;
            LocalEndPoint = localEndPoint;
            RemoteEndPoint = remoteEndPoint;

            ServerDetected = LocalEndPoint.Address.IsLocal() && !RemoteEndPoint.Address.IsLocal();

            _packetType = packetType;

            Packets = new List<TimedPacket>();
            Flows = new List<TcpFlow>();
        }

        public void AddPacket(TimedPacket timedPacket)
        {
            lock (Packets)
            {
                Packets.Add(timedPacket);
            }

            TotalData += (ulong)timedPacket.Packet.PayloadData.Length;

            TcpFlow tcpFlow = null;
            var isNewFlow = false;

            switch (timedPacket.Sender)
            {
                case SenderEnum.Client:
                    tcpFlow = Flows.Find((flow) => flow.InitialAck == timedPacket.Packet.AcknowledgmentNumber);
                    break;
                case SenderEnum.Server:
                    tcpFlow = Flows.Find((flow) => flow.InitialSeq == timedPacket.Packet.SequenceNumber);
                    break;
            }

            if (tcpFlow == null)
            {
                tcpFlow = new TcpFlow();
                Flows.Add(tcpFlow);
                isNewFlow = true;
            }

            tcpFlow.AddPacket(timedPacket);

            if (isNewFlow)
            {
                OnNewFlow?.Invoke(tcpFlow);
            }
            else
            {
                OnFlowUpdate?.Invoke(tcpFlow);
            }

            OnPacketArrive?.Invoke(timedPacket);
        }

        public void AddPacket(TcpPacket packet, PosixTimeval timeVal, SenderEnum sender)
        {
            AddPacket(new TimedPacket(packet, timeVal, sender));
        }

        public override string ToString()
        {
            return $"{LocalEndPoint.Address}:{LocalEndPoint.Port} -> {RemoteEndPoint.Address}:{RemoteEndPoint.Port}";
        }

        public override bool Equals(object obj)
        {
            var info = (ConnectionInfo)obj;

            if (info == null)
            {
                return false;
            }

            return EqualsTo(info.ProtocolName, info.LocalEndPoint, info.RemoteEndPoint);
        }

        public bool EqualsTo(string protocolName, IPEndPoint localEndPoint, IPEndPoint remoteEndPoint)
        {
            return ProtocolName == protocolName && LocalEndPoint.Equals(localEndPoint) && RemoteEndPoint.Equals(remoteEndPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = hash * 23 + AppName.GetHashCode();
                hash = hash * 23 + ProtocolName.GetHashCode();
                hash = hash * 23 + LocalEndPoint.GetHashCode();
                hash = hash * 23 + RemoteEndPoint.GetHashCode();

                return hash;
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

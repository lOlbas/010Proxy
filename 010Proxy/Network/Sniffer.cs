using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.Npcap;
using System;

namespace _010Proxy.Network
{
    public class Sniffer
    {
        public CaptureDeviceList Devices { get; private set; }

        private ICaptureDevice _activeDevice;

        public event Action<Packet, PosixTimeval> OnPacketReceive;

        public Sniffer()
        {
            Init();
        }

        public void Init()
        {
            Devices = CaptureDeviceList.Instance;
        }

        public void StartCapture(ICaptureDevice device)
        {
            _activeDevice = device;

            _activeDevice.OnPacketArrival += OnPacketArrival;

            var readTimeoutMilliseconds = 1000;

            switch (_activeDevice)
            {
                case NpcapDevice nPcap:
                    nPcap.Open(OpenFlags.DataTransferUdp | OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
                    break;
                case LibPcapLiveDevice livePcapDevice:
                    livePcapDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                    break;
                default:
                    throw new InvalidOperationException("Unknown device type of " + _activeDevice.GetType());
            }

            _activeDevice.StartCapture();
        }

        public void StopCapture()
        {
            _activeDevice?.StopCapture();
        }

        public void OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            OnPacketReceive?.Invoke(packet, e.Packet.Timeval);
        }
    }
}

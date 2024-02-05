using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public class PacketCaptureService(ILogger<PacketCaptureService> logger, ChannelWriter<CaptureWrapper> writer) : IPacketCaptureService
{
    private readonly CaptureDeviceList _devices = CaptureDeviceList.Instance;
    
    public Task StartAsync()
    {
        foreach (var device in _devices)
        {
            logger.LogDebug("Starting packet capture service on device {Device}", device.Description); 
            device.OnPacketArrival += OnPacketArrival;
        
            device.Open(DeviceModes.Promiscuous); 
            device.StartCapture();
        }
        return Task.CompletedTask;
    }

    private void OnPacketArrival(object sender, PacketCapture e)
    {
        logger.LogDebug("Packet arrived");
        var capture = new CaptureWrapper
        {
            DeviceName = e.Device.Description,
            Capture = e.GetPacket(),
            Device = e.Device,
            Header = e.Header
        };
        
        if (e.GetPacket().LinkLayerType == LinkLayers.Ethernet)
        {
            var ethernetPacket = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data) as EthernetPacket;
            if (ethernetPacket != null)
            {
                logger.LogDebug("Ethernet packet arrived");
                capture.Inbound = ethernetPacket.DestinationHardwareAddress.ToString() == "00:00:00:00:00:00";
            }
        }
        
        writer.WriteAsync(capture).GetAwaiter().GetResult();
    }

    public Task StopAsync()
    {
        foreach (var device in _devices)
        {
            logger.LogDebug("Stopping packet capture service on device {Device}", device.Description);
            device.StopCapture();
            device.Close();
        }
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        foreach (var device in _devices)
        {
            device.OnPacketArrival -= OnPacketArrival;
            device.Dispose();
        }
        return new ValueTask();
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
}
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public class PacketCaptureService(ILogger<PacketCaptureService> logger, ChannelWriter<RawCapture> writer) : IPacketCaptureService
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
        writer.WriteAsync(e.GetPacket()).GetAwaiter().GetResult();
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
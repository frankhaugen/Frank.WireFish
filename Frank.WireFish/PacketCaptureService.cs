using Frank.WireFish.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpPcap;

namespace Frank.WireFish;

public class PacketCaptureService(ILogger<PacketCaptureService> logger, IPacketHandler packetHandler) : IHostedService
{
    private readonly CaptureDeviceList _devices = CaptureDeviceList.Instance;

    /// <inheritdoc />
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _devices)
        {
            // Open the device for capturing
            device.Open(DeviceModes.Promiscuous);
            
            // Start capturing packets
            device.OnPacketArrival += packetHandler.HandlePacket;
            device.StartCapture();
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _devices)
        {
            logger.LogDebug("Stopping packet capture service on device {Device}", device.Description);
            device.StopCapture();
            device.Close();
        }
        await Task.CompletedTask;
    }
}
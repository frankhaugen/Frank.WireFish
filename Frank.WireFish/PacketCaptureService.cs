using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharpPcap;

namespace Frank.WireFish;

public class PacketCaptureService(IOptions<PacketCaptureSettings> options, ILogger<PacketCaptureService> logger, IPacketHandler packetHandler) : IHostedService
{
    private readonly CaptureDeviceList _devices = CaptureDeviceList.Instance;
    

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _devices)
        {
            // Open the device for capturing
            device.Open(DeviceModes.Promiscuous);

            // Set a filter to capture only TCP packets
            device.Filter = options.Value.Filter;
            
            // Start capturing packets
            device.OnPacketArrival += packetHandler.HandlePacket;
            device.StartCapture();
        }
        
        await Task.CompletedTask;
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
using System.Threading.Channels;

using PacketDotNet;

using SharpPcap.LibPcap;

namespace Frank.WireFish.Internals;

internal class PacketHandler(ChannelWriter<DevicePacket> writer)
{
    public async Task HandlePacket(LibPcapLiveDevice device, Packet packet)
    {
        var devicePacket = new DevicePacket(device, packet, DateTime.UtcNow);
        var valueTask = writer.WriteAsync(devicePacket);
        valueTask.AsTask().Wait();
    }
}
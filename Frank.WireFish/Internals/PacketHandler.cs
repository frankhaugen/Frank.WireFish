using System.Threading.Channels;

using PacketDotNet;

using SharpPcap;

namespace Frank.WireFish.Internals;

internal class PacketHandler(ChannelWriter<DevicePacket> writer)
{
    public void HandlePacket(object sender, PacketCapture e)
    {
        var device = e.Device;
        var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
        var devicePacket = new DevicePacket(device, packet, DateTime.UtcNow);
        var valueTask = writer.WriteAsync(devicePacket);
        valueTask.AsTask().Wait();
    }
}
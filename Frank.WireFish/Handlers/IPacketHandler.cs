using SharpPcap;

namespace Frank.WireFish.Handlers;

public interface IPacketHandler
{
    void HandlePacket(object sender, PacketCapture e);
}
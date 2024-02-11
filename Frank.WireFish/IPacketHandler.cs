using SharpPcap;

namespace Frank.WireFish;

public interface IPacketHandler
{
    void HandlePacket(object sender, PacketCapture e);
}
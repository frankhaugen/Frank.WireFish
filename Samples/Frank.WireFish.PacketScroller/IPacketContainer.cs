namespace Frank.WireFish.PacketScroller;

public interface IPacketContainer
{
    void Add(DevicePacket packet);
    IEnumerable<DevicePacket> GetPackets();
    void Clear();
    bool IsEmpty();
    int Count();
}
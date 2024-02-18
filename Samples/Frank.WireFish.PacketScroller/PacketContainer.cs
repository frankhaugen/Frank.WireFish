using System.Collections.Concurrent;

namespace Frank.WireFish.PacketScroller;

public class PacketContainer : IPacketContainer
{
    private readonly ConcurrentBag<DevicePacket> _packets = new();
    
    public void Add(DevicePacket packet)
    {
        ArgumentNullException.ThrowIfNull(packet);
        _packets.Add(packet);
    }
    
    public IEnumerable<DevicePacket> GetPackets() => _packets.ToArray();
    
    public void Clear() => _packets.Clear();
    
    public bool IsEmpty() => _packets.IsEmpty;
    
    public int Count() => _packets.Count;
}
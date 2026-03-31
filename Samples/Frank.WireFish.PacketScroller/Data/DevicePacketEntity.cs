using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public class DevicePacketEntity : RealmObject
{
    public string? Device { get; set; }
    public string PacketContentType { get; set; } = string.Empty;
    public byte[]? PacketData { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
}
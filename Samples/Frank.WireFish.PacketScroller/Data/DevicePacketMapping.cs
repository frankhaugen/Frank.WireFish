using System.Text;

using Frank.Mapping;

using PacketDotNet;

namespace Frank.WireFish.PacketScroller.Data;

public class DevicePacketMapping : IMappingDefinition<DevicePacket, DevicePacketEntity>
{
    public DevicePacketEntity Map(DevicePacket source)
    {
        var packet = source.Packet as EthernetPacket;
        
        if (packet is not null)
        {
            return new DevicePacketEntity()
            {
                Device = source.Device.ToString(),
                PacketContentType = Encoding.UTF8.GetString(packet.HeaderData),
                PacketData = packet.PayloadData,
                Timestamp = source.Timestamp
            };
        }
        
        
        return new DevicePacketEntity()
        {
            Device = source.Device.ToString(),
            PacketContentType = Encoding.UTF8.GetString(source.Packet.HeaderData),
            PacketData = source.Packet.PayloadData,
            Timestamp = source.Timestamp
        };
    }
}
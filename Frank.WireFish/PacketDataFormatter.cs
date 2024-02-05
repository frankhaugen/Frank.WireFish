using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public static class PacketDataFormatter
{
    public static string Format(RawCapture capture)
    {
        var packet = Packet.ParsePacket(capture.LinkLayerType, capture.Data);
        if (packet is EthernetPacket ethernetPacket)
        {
            return Format(ethernetPacket);
        }
        return packet.ToString();
    }

    private static string Format(EthernetPacket ethernetPacket)
    {
        return $"Ethernet: {ethernetPacket.SourceHardwareAddress} -> {ethernetPacket.DestinationHardwareAddress} {ethernetPacket.PayloadPacket}";
    }
}
using System.Threading.Channels;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public class PacketHandler(ChannelWriter<CapturedTcpPacket> writer) : IPacketHandler
{
    public void HandlePacket(object sender, PacketCapture e)
    {
        var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
        var ipPacket = packet.Extract<IPPacket>();
        var tcpPacket = ipPacket.Extract<TcpPacket>();

        // Process the packet
        var sourceIp = ipPacket.SourceAddress;
        var destinationIp = ipPacket.DestinationAddress;
        var sourcePort = tcpPacket.SourcePort;
        var destinationPort = tcpPacket.DestinationPort;

        var packetDto = new CapturedTcpPacket(new IpPort(sourceIp.ToString(), sourcePort),
            new IpPort(destinationIp.ToString(), destinationPort),
            packet,
            ipPacket,
            tcpPacket);
        
        var valueTask = writer.WriteAsync(packetDto);
        valueTask.AsTask().Wait();
    }
}
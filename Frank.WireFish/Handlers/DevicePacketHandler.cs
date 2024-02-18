using System.Threading.Channels;
using Frank.WireFish.Models;
using Microsoft.Extensions.Hosting;
using PacketDotNet;

namespace Frank.WireFish.Handlers;

public class DevicePacketHandler(ChannelReader<DevicePacket> reader, ChannelWriter<IrcPacket> writer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var packet in reader.ReadAllAsync(stoppingToken))
        {
            var protocolType = IdentifyProtocol(packet);
            var ircPacket = protocolType switch
            {
                ProtocolType.Tcp => HandleTcpPacket(packet),
                _ => null
            };
            
            if (ircPacket is not null) 
                await writer.WriteAsync(ircPacket, stoppingToken);
        }
    }

    private static IrcPacket? HandleTcpPacket(DevicePacket packet)
    {
        var destinationPort = packet.Packet.Extract<TcpPacket>().DestinationPort;
        var sourcePort = packet.Packet.Extract<TcpPacket>().SourcePort;

        return destinationPort == 6667 || sourcePort == 6667 ? HandleTcpIrcPacket(packet) : null;
    }

    private static IrcPacket HandleTcpIrcPacket(DevicePacket packet)
    {
        var sourceIp = packet.Packet.Extract<IPPacket>().SourceAddress;
        var destinationIp = packet.Packet.Extract<IPPacket>().DestinationAddress;
        var payload = packet.Packet.Extract<TcpPacket>().PayloadData.ToString();
        
        return new IrcPacket(packet.Timestamp, sourceIp.ToString(), destinationIp.ToString(), payload);
    }

    private static ProtocolType IdentifyProtocol(DevicePacket devicePacket)
    {
        return devicePacket.Packet switch
        {
            TcpPacket _ => ProtocolType.Tcp,
            IPv4Packet _ => ProtocolType.IPv4,
            IPv6Packet _ => ProtocolType.IPv6,
            UdpPacket _ => ProtocolType.Udp,
            _ => ProtocolType.Encapsulation
        };
    }
}
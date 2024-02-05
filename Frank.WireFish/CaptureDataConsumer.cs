using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public class CaptureDataConsumer(
    ILogger<CaptureDataConsumer> logger, 
    ChannelReader<RawCapture> reader, 
    ChannelWriter<EthernetPacket> ethernetPacketWriter,
    ChannelWriter<IPPacket> ipPacketWriter,
    ChannelWriter<TcpPacket> tcpPacketWriter,
    ChannelWriter<UdpPacket> udpPacketWriter,
    ChannelWriter<InternetPacket> internetPacketWriter,
    ChannelWriter<Ieee8021QPacket> ieee8021QPacketWriter
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            // Switch on the packet type as cast
            switch (item.GetPacket())
            {
                case Ieee8021QPacket ieee8021QPacket:
                    await ieee8021QPacketWriter.WriteAsync(ieee8021QPacket, stoppingToken);
                    break;
                case EthernetPacket ethernetPacket:
                    await ethernetPacketWriter.WriteAsync(ethernetPacket, stoppingToken);
                    break;
                case IPv4Packet ipv4Packet:
                    await ipPacketWriter.WriteAsync(ipv4Packet, stoppingToken);
                    break;
                case IPv6Packet ipv6Packet:
                    await ipPacketWriter.WriteAsync(ipv6Packet, stoppingToken);
                    break;
                case IPPacket ipPacket:
                    await ipPacketWriter.WriteAsync(ipPacket, stoppingToken);
                    break;
                case TcpPacket tcpPacket:
                    await tcpPacketWriter.WriteAsync(tcpPacket, stoppingToken);
                    break;
                case UdpPacket udpPacket:
                    await udpPacketWriter.WriteAsync(udpPacket, stoppingToken);
                    break;
                case InternetPacket internetPacket:
                    await internetPacketWriter.WriteAsync(internetPacket, stoppingToken);
                    break;
                default:
                    logger.LogWarning("Unhandled packet type {PacketType}", item.GetPacket().ToString(StringOutputType.Normal));
                    break;
            }
        }
    }
}
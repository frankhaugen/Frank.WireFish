using System.Text;
using System.Threading.Channels;
using Frank.WireFish.Models;
using Microsoft.Extensions.Hosting;
using PacketDotNet;

namespace Frank.WireFish.Handlers;

public class IrcDevicePacketHandler(ChannelReader<IrcDevicePacket> reader, ChannelWriter<IrcPacket> writer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var packet in reader.ReadAllAsync(stoppingToken))
        {
            var ipPacket = packet.Packet.Extract<IPPacket>();
            var tcpPacket = ipPacket.Extract<TcpPacket>();
            
            var sourceIp = ipPacket.SourceAddress;
            var destinationIp = ipPacket.DestinationAddress;
            
            var ircPacket = new IrcPacket(packet.Timestamp, sourceIp.ToString(), destinationIp.ToString(), tcpPacket.PayloadData.ToString());
            
            var valueTask = writer.WriteAsync(ircPacket, stoppingToken);
            await valueTask.AsTask().WaitAsync(stoppingToken);
        }
    }
}

public class IrcPacketHandler(ChannelReader<IrcPacket> reader, ChannelWriter<FileWriteRequest> writer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var packet in reader.ReadAllAsync(stoppingToken))
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Timestamp: {packet.Timestamp}");
            stringBuilder.AppendLine($"Source IP: {packet.Source}");
            stringBuilder.AppendLine($"Destination IP: {packet.Destination}");
            stringBuilder.AppendLine($"Message: {packet.Message}");
            
            var file = new FileInfo("irc_packets.txt");
            var fileWriteRequest = new FileWriteRequest(file, stringBuilder.ToString());
            
            var valueTask = writer.WriteAsync(fileWriteRequest, stoppingToken);
            await valueTask.AsTask().WaitAsync(stoppingToken);
        }
    }
}
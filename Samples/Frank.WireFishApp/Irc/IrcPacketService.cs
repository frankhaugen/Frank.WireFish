using System.Text;
using System.Threading.Channels;

using Frank.WireFishApp.FileWriting;

namespace Frank.WireFishApp.Irc;

public class IrcPacketService(ChannelReader<IrcPacket> reader, ChannelWriter<FileWriteRequest> writer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var packet in reader.ReadAllAsync(stoppingToken))
        {
            if (string.IsNullOrWhiteSpace(packet.Message))
                continue;
            
            var rowBuilder = new StringBuilder();
            rowBuilder.Append(packet.Timestamp);
            rowBuilder.Append('\t');
            rowBuilder.Append(packet.Device);
            rowBuilder.Append('\t');
            rowBuilder.Append(packet.Source);
            rowBuilder.Append('\t');
            rowBuilder.Append(packet.Destination);
            rowBuilder.Append('\t');
            rowBuilder.Append(packet.Message);
            var row = rowBuilder.ToString();
            
            var file = new FileInfo("irc_packets.csv");
            var fileWriteRequest = new FileWriteRequest(file, row);
            
            var valueTask = writer.WriteAsync(fileWriteRequest, stoppingToken);
            await valueTask.AsTask().WaitAsync(stoppingToken);
        }
    }
}
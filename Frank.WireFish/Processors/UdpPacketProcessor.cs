using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PacketDotNet;

namespace Frank.WireFish.Processors;

public class UdpPacketProcessor(ILogger<UdpPacketProcessor> logger, ChannelReader<UdpPacket> reader, ChannelWriter<Tuple<FileInfo, string>> writer)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            var fileInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, "dump", $"{nameof(UdpPacket)}s_dump.txt"));
            await writer.WriteAsync(new Tuple<FileInfo, string>(fileInfo, PacketDataFormatter.Format(item)), stoppingToken);
        }
    }
}
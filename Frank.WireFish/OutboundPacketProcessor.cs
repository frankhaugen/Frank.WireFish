using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.WireFish;

public class OutboundPacketProcessor(ILogger<OutboundPacketProcessor> logger, ChannelReader<CapturedOutboundPacket> reader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            logger.LogInformation("Outbound packet received");
        }
    }
}
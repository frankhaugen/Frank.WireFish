using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.WireFish;

public class InboundPacketProcessor(ILogger<InboundPacketProcessor> logger, ChannelReader<CapturedInboundPacket> reader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            logger.LogInformation("Inbound packet received");
        }
    }
}
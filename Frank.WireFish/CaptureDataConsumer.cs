using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharpPcap;

namespace Frank.WireFish;

public class CaptureDataConsumer(ILogger<CaptureDataConsumer> logger, ChannelReader<CaptureWrapper> reader, ChannelWriter<CapturedOutboundPacket> outboundWriter, ChannelWriter<CapturedInboundPacket> inboundWriter)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            if (item.Inbound.GetValueOrDefault())
            {
                await outboundWriter.WriteAsync(new CapturedOutboundPacket { Capture = item }, stoppingToken);
            }
            else
            {
                await inboundWriter.WriteAsync(new CapturedInboundPacket { Capture = item }, stoppingToken);
            }
        }
    }
}
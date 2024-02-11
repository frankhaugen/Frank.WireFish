using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

namespace Frank.WireFish.Processors;

public abstract class BaseProcessor<TIn, TOut>(ChannelReader<TIn> reader, ChannelWriter<TOut> writer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            await writer.WriteAsync(ProcessItem(item), stoppingToken);
        }
    }

    protected abstract TOut ProcessItem(TIn item);
}
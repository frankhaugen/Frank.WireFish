﻿using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Frank.WireFish;

public class OutboundPacketProcessor(ILogger<OutboundPacketProcessor> logger, ChannelReader<CapturedOutboundPacket> reader, ChannelWriter<Tuple<FileInfo, string>> writer)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in reader.ReadAllAsync(stoppingToken))
        {
            var fileInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, $"outbound.txt"));
            await writer.WriteAsync(new Tuple<FileInfo, string>(fileInfo, PacketDataFormatter.Format(item.Capture.Capture)), stoppingToken);
        }
    }
}
﻿using System.Threading.Channels;

using Microsoft.Extensions.Hosting;

namespace Frank.WireFish.Internals;

internal class DevicePacketHandler(ChannelReader<DevicePacket> reader, IEnumerable<IPacketHandler> handlers) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var packet in reader.ReadAllAsync(stoppingToken))
        {
            foreach (var handler in handlers)
            {
                if (handler.CanHandle(packet))
                {
                    await handler.HandleAsync(packet, stoppingToken);
                }
            }
        }
    }
}
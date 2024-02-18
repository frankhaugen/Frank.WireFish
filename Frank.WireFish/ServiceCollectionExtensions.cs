using Frank.Channels.DependencyInjection;
using Frank.WireFish.Handlers;
using Frank.WireFish.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        // Channels
        services.AddChannel<DevicePacket>();
        services.AddChannel<IrcDevicePacket>();
        services.AddChannel<IrcPacket>();

        // Initial packet capture service
        services.AddHostedService<PacketCaptureService>();
        
        // Handler of initial packet captures from the packet capture service
        services.AddSingleton<IPacketHandler, PacketHandler>();

        // Device packet handler, that handles the packets after some processing
        services.AddHostedService<DevicePacketHandler>();
        
        // IRC packet handler, that handles the packets that are IRC related
        services.AddHostedService<IrcDevicePacketHandler>();
        
        // IRC packet handler, that handles the packets that are IRC related
        services.AddHostedService<IrcPacketHandler>();
        
        // File writer, that writes the packets to a file
        services.AddHostedService<FileWriter>();

        return services;
    }
}
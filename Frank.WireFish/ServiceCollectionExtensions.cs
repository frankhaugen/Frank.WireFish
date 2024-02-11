using Frank.Channels.DependencyInjection;
using Frank.WireFish.Models;
using Frank.WireFish.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddSingleton<IPacketHandler, PacketHandler>();

        services.AddChannel<CapturedTcpPacket>();
        services.AddChannel<FileWriteRequest>();

        services.AddHostedService<PacketCaptureService>();
        services.AddHostedService<FileWriter>();
        
        services.AddHostedService<CapturedTcpPacketProcessor>();

        return services;
    }
}
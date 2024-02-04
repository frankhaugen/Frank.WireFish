using Frank.Channels.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddSingleton<IPacketCaptureService, PacketCaptureService>();

        services.AddChannel<CaptureWrapper>();
        services.AddChannel<CapturedInboundPacket>();
        services.AddChannel<CapturedOutboundPacket>();
        
        services.AddHostedService<CaptureDataConsumer>();
        services.AddHostedService<InboundPacketProcessor>();
        services.AddHostedService<OutboundPacketProcessor>();
        
        return services;
    }
}
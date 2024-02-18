using Frank.Channels.DependencyInjection;
using Frank.WireFish.Internals;

using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWireFish(this IServiceCollection services, Action<IWireFishBuilder> wireFishBuilder)
    {
        services.AddPacketCaptureService();
        var builder = new WireFishBuilder(services);
        wireFishBuilder(builder);
        return services;
    }
    
    internal static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddChannel<DevicePacket>();
        services.AddHostedService<PacketCaptureService>();
        services.AddSingleton<PacketHandler>();
        services.AddHostedService<DevicePacketHandler>();
        return services;
    }
}
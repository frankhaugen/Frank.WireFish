using Frank.Channels.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddSingleton<IPacketCaptureService, PacketCaptureService>();

        services.AddChannel<RawCapture>();
        services.AddChannel<IPPacket>();
        services.AddChannel<EthernetPacket>();
        services.AddChannel<TcpPacket>();
        services.AddChannel<UdpPacket>();
        services.AddChannel<InternetPacket>();
        services.AddChannel<Ieee8021QPacket>();
        services.AddChannel<Tuple<FileInfo, string>>();
        
        services.AddHostedService<CaptureDataConsumer>();
        services.AddHostedService<EthernetPacketProcessor>();
        services.AddHostedService<Ieee8021QPacketProcessor>();
        services.AddHostedService<IPPacketProcessor>();
        services.AddHostedService<TcpPacketProcessor>();
        services.AddHostedService<UdpPacketProcessor>();
        services.AddHostedService<InternetPacketProcessor>();
        services.AddHostedService<FileWriter>();
        
        return services;
    }
}
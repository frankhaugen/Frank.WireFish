using Frank.Channels.DependencyInjection;
using Frank.WireFish.Processors;
using Microsoft.Extensions.DependencyInjection;
using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPacketCaptureService(this IServiceCollection services)
    {
        services.AddHostedService<PacketCaptureService>();
        services.AddChannel<CapturedTcpPacket>();
        services.AddSingleton<IPacketHandler, PacketHandler>();

        services.AddChannel<RawCapture>();
        services.AddChannel<IPPacket>();
        services.AddChannel<EthernetPacket>();
        services.AddChannel<TcpPacket>();
        services.AddChannel<UdpPacket>();
        services.AddChannel<InternetPacket>();
        services.AddChannel<Ieee8021QPacket>();
        services.AddChannel<Packet>();
        services.AddChannel<NullPacket>();
        services.AddChannel<Tuple<FileInfo, string>>();

        services.AddHostedService<FileWriter>();
        
        services.AddHostedService<CaptureDataProcessor>();
        services.AddHostedService<EthernetPacketProcessor>();
        services.AddHostedService<Ieee8021QPacketProcessor>();
        services.AddHostedService<IPPacketProcessor>();
        services.AddHostedService<TcpPacketProcessor>();
        services.AddHostedService<UdpPacketProcessor>();
        services.AddHostedService<InternetPacketProcessor>();
        services.AddHostedService<PacketProcessor>();
        services.AddHostedService<NullPacketProcessor>();


        return services;
    }
}
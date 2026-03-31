
using Frank.Mapping;
using Frank.WireFish.PacketScroller.Data;

using PacketDotNet;

using Spectre.Console;

namespace Frank.WireFish.PacketScroller;

public class PacketCatogorizingHandler : IPacketHandler
{
    private readonly IRepository<DevicePacketEntity> _repository;
    private readonly IMappingDefinition<DevicePacket, DevicePacketEntity> _mapping;
    private readonly IAnsiConsole _console;
    
    private readonly Dictionary<int, Color> _colors = new()
    {
        { 80, Color.Red },
        { 443, Color.Green },
        { 8080, Color.Blue }
    };

    public PacketCatogorizingHandler(IAnsiConsole console, IRepository<DevicePacketEntity> repository, IMappingDefinition<DevicePacket, DevicePacketEntity> mapping)
    {
        _console = console;
        _repository = repository;
        _mapping = mapping;
    }

    /// <inheritdoc />
    public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
    {
        try
        {
            var tcpPacket = packet.Packet.Extract<IPPacket>().PayloadPacket.Extract<TcpPacket>();
            var color = GetColor(tcpPacket);
            
            _console.MarkupLine($"[{color.ToMarkup()}]{packet.Timestamp}\t{packet.Packet.Extract<IPPacket>().PayloadPacket.Extract<TcpPacket>().DestinationPort}\t{packet.Device.Name}[/]");
            
            var entity = _mapping.Map(packet);
            await _repository.AddAsync(entity);
        }
        catch (Exception e)
        {
            
        }

        await Task.CompletedTask;
    }

    private Color GetColor(TcpPacket tcpPacket)
    {
        var port = tcpPacket.DestinationPort;
        
        Color color;
        
        if (_colors.TryGetValue(port, out color))
        {
            return color;
        }
        else
        {
            color = CreateVibrantColorFromPort(port);
        }
        
        _colors.Add(port, color);
        
        return color;
    }
    
    private static Color CreateVibrantColorFromPort(int port)
    {
        const int maxColorValue = 255; // Extracted constant
        var colorValue = maxColorValue - (port % maxColorValue); // Calculation to make lower ports more vibrant
        return new Color((byte)colorValue, (byte)colorValue, (byte)colorValue);
    }

    /// <inheritdoc />
    public bool CanHandle(DevicePacket packet) => true;
}

using PacketDotNet;

using Spectre.Console;

namespace Frank.WireFish.PacketScroller;

public class PacketCatogorizingHandler : IPacketHandler
{
    private readonly IPacketContainer _packetContainer;
    private readonly IAnsiConsole _console;
    
    private readonly Dictionary<int, Color> _colors = new()
    {
        { 80, Color.Red },
        { 443, Color.Green },
        { 8080, Color.Blue }
    };

    public PacketCatogorizingHandler(IPacketContainer packetContainer, IAnsiConsole console)
    {
        _packetContainer = packetContainer;
        _console = console;
    }

    /// <inheritdoc />
    public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
    {
        try
        {
            var tcpPacket = packet.Packet.Extract<IPPacket>().PayloadPacket.Extract<TcpPacket>();
            var color = GetColor(tcpPacket);
            
            _console.MarkupLine($"[{color.ToMarkup()}]{packet.Device.Name}\t{packet.Timestamp}\t{packet.Packet.Extract<IPPacket>().PayloadPacket.Extract<TcpPacket>().DestinationPort}[/]");
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
            color = new Color((byte)port, (byte)(port >> 8), (byte)(port >> 16));
        }
        
        _colors.Add(port, color);
        
        return color;
    }

    /// <inheritdoc />
    public bool CanHandle(DevicePacket packet) => true;
}
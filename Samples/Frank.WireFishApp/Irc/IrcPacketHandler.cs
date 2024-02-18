using System.Text;
using System.Threading.Channels;

using Frank.WireFish;

using PacketDotNet;

namespace Frank.WireFishApp.Irc;

public class IrcPacketHandler(ChannelWriter<IrcPacket> ircPacketWriter) : IPacketHandler
{
    /// <inheritdoc />
    public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
    {
        
        var destinationPort = packet.Packet.Extract<TcpPacket>().DestinationPort;
        var sourcePort = packet.Packet.Extract<TcpPacket>().SourcePort;
        var tcpPacket = packet.Packet.Extract<TcpPacket>();
        var message = Encoding.UTF8.GetString(tcpPacket.PayloadData);
        var ircPacket = new IrcPacket(packet.Timestamp, sourcePort.ToString(), destinationPort.ToString(), packet.Device.Description, message);
        await ircPacketWriter.WriteAsync(ircPacket, cancellationToken);     
    }

    /// <inheritdoc />
    public bool CanHandle(DevicePacket packet)
    {
        try
        {
            var destinationPort = packet.Packet.Extract<TcpPacket>().DestinationPort;
            var sourcePort = packet.Packet.Extract<TcpPacket>().SourcePort;
            return (destinationPort == 6667 || sourcePort == 6667) && packet.Packet.Extract<TcpPacket>().PayloadData.Length > 0;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
using System.Threading.Channels;
using Frank.WireFish.Models;
using Microsoft.Extensions.Logging;

namespace Frank.WireFish.Processors;

public class CapturedTcpPacketProcessor(ILogger<CapturedTcpPacketProcessor> logger, ChannelReader<CapturedTcpPacket> reader, ChannelWriter<FileWriteRequest> writer) : BaseProcessor<CapturedTcpPacket, FileWriteRequest>(reader, writer)
{
    /// <inheritdoc />
    protected override FileWriteRequest ProcessItem(CapturedTcpPacket item)
    {
        var packetType = item.Packet.GetType().Name;
        
        logger.LogInformation("Processing {PacketType} packet", packetType);
        
        var fileInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, "dump", $"{packetType}s_dump.txt"));
        var packetData = PacketDataFormatter.Format(item.Packet);
        
        return new FileWriteRequest(fileInfo, packetData);
    }
}
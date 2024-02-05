using System.Text;
using PacketDotNet;

namespace Frank.WireFish;

public static class PacketDataFormatter
{
    public static string Format(EthernetPacket capture, char delimiter = '\t')
    {
        var cells = new List<string?>
        {
            capture.SourceHardwareAddress?.ToString(),
            capture.DestinationHardwareAddress?.ToString(),
            Encoding.UTF8.GetString(capture.PayloadData ?? Array.Empty<byte>())
        };
        
        return string.Join(delimiter, cells);
    }
    
    public static string Format(IPPacket capture, char delimiter = '\t')
    {
        var cells = new List<string?>
        {
            capture.SourceAddress?.ToString(),
            capture.DestinationAddress?.ToString(),
            Encoding.UTF8.GetString(capture.PayloadData ?? Array.Empty<byte>())
        };
        
        return string.Join(delimiter, cells);
    }
    
    public static string Format(TcpPacket capture, char delimiter = '\t')
    {
        var cells = new List<string>
        {
            capture.SourcePort.ToString(),
            capture.DestinationPort.ToString(),
            Encoding.UTF8.GetString(capture.PayloadData ?? Array.Empty<byte>())
        };
        
        return string.Join(delimiter, cells);
    }
    
    public static string Format(UdpPacket capture, char delimiter = '\t')
    {
        var cells = new List<string>
        {
            capture.SourcePort.ToString(),
            capture.DestinationPort.ToString(),
            Encoding.UTF8.GetString(capture.PayloadData ?? Array.Empty<byte>())
        };
        
        return string.Join(delimiter, cells);
    }
    
    public static string Format(InternetPacket capture, char delimiter = '\t')
    {
        var cells = new List<string>
        {
            string.Empty,
            string.Empty,
            Encoding.UTF8.GetString(Array.Empty<byte>())
        };
        
        return string.Join(delimiter, cells);
    }
    
}
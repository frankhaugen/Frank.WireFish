using System.Net;
using System.Net.NetworkInformation;

using PacketDotNet;

namespace Frank.WireFish;

/// <summary>
/// Provides extension methods for the DevicePacket record to facilitate networking tasks.
/// </summary>
public static class DevicePacketExtensions
{
    /// <summary>
    /// Gets the source IP address from the packet.
    /// </summary>
    public static IPAddress GetSourceIPAddress(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPv4Packet>();
        if (ipPacket != null)
            return ipPacket.SourceAddress;

        var ipv6Packet = devicePacket.Packet.Extract<IPv6Packet>();
        return ipv6Packet?.SourceAddress;
    }

    /// <summary>
    /// Gets the destination IP address from the packet.
    /// </summary>
    public static IPAddress GetDestinationIPAddress(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPv4Packet>();
        if (ipPacket != null)
            return ipPacket.DestinationAddress;

        var ipv6Packet = devicePacket.Packet.Extract<IPv6Packet>();
        return ipv6Packet?.DestinationAddress;
    }

    /// <summary>
    /// Gets the source port from the packet if available.
    /// </summary>
    public static int? GetSourcePort(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
            return tcpPacket.SourcePort;

        var udpPacket = devicePacket.Packet.Extract<UdpPacket>();
        return udpPacket?.SourcePort;
    }

    /// <summary>
    /// Gets the destination port from the packet if available.
    /// </summary>
    public static int? GetDestinationPort(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
            return tcpPacket.DestinationPort;

        var udpPacket = devicePacket.Packet.Extract<UdpPacket>();
        return udpPacket?.DestinationPort;
    }

    /// <summary>
    /// Gets the protocol type of the packet (e.g., TCP, UDP).
    /// </summary>
    public static string GetProtocol(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        return ipPacket?.Protocol.ToString();
    }

    /// <summary>
    /// Determines if the packet is a TCP packet.
    /// </summary>
    public static bool IsTcp(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<TcpPacket>() != null;
    }

    /// <summary>
    /// Determines if the packet is a UDP packet.
    /// </summary>
    public static bool IsUdp(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<UdpPacket>() != null;
    }

    /// <summary>
    /// Gets the TCP flags from the packet if it's a TCP packet.
    /// </summary>
    public static ushort? GetTcpFlags(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        return tcpPacket?.Flags;
    }

    /// <summary>
    /// Gets the payload data of the packet.
    /// </summary>
    public static byte[]? GetPayloadData(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        return ipPacket?.PayloadData;
    }

    /// <summary>
    /// Determines if the packet is an HTTP packet based on common HTTP ports.
    /// </summary>
    public static bool IsHttp(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            int srcPort = tcpPacket.SourcePort;
            int dstPort = tcpPacket.DestinationPort;
            return srcPort == 80 || dstPort == 80 || srcPort == 8080 || dstPort == 8080 || srcPort == 443 || dstPort == 443;
        }
        return false;
    }

    /// <summary>
    /// Determines if the packet is a DNS packet based on common DNS ports.
    /// </summary>
    public static bool IsDnsPacket(this DevicePacket devicePacket)
    {
        var udpPacket = devicePacket.Packet.Extract<UdpPacket>();
        if (udpPacket != null)
        {
            int srcPort = udpPacket.SourcePort;
            int dstPort = udpPacket.DestinationPort;
            return srcPort == 53 || dstPort == 53;
        }
        return false;
    }

    /// <summary>
    /// Gets the source MAC address from the packet.
    /// </summary>
    public static PhysicalAddress? GetMacSourceAddress(this DevicePacket devicePacket)
    {
        var ethernetPacket = devicePacket.Packet.Extract<EthernetPacket>();
        return ethernetPacket?.SourceHardwareAddress;
    }

    /// <summary>
    /// Gets the destination MAC address from the packet.
    /// </summary>
    public static PhysicalAddress? GetMacDestinationAddress(this DevicePacket devicePacket)
    {
        var ethernetPacket = devicePacket.Packet.Extract<EthernetPacket>();
        return ethernetPacket?.DestinationHardwareAddress;
    }

    /// <summary>
    /// Determines if the packet is an IPv4 packet.
    /// </summary>
    public static bool IsIpV4Packet(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<IPv4Packet>() != null;
    }

    /// <summary>
    /// Determines if the packet is an IPv6 packet.
    /// </summary>
    public static bool IsIpV6Packet(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<IPv6Packet>() != null;
    }

    /// <summary>
    /// Determines if the packet is an ARP packet.
    /// </summary>
    public static bool IsArpPacket(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<ArpPacket>() != null;
    }

    /// <summary>
    /// Gets a summary of the packet.
    /// </summary>
    public static string GetPacketSummary(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.ToString(StringOutputType.VerboseColored);
    }

    /// <summary>
    /// Gets the length of the packet in bytes.
    /// </summary>
    public static int GetPacketLength(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.TotalPacketLength;
    }

    /// <summary>
    /// Converts the packet data to a hexadecimal string representation.
    /// </summary>
    public static string ToHexDump(this DevicePacket devicePacket)
    {
        var bytes = devicePacket.Packet.Bytes;
        return BitConverter.ToString(bytes).Replace("-", " ");
    }

    /// <summary>
    /// Gets the TCP sequence number if it's a TCP packet.
    /// </summary>
    public static uint? GetTcpSequenceNumber(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        return tcpPacket?.SequenceNumber;
    }

    /// <summary>
    /// Gets the TCP acknowledgment number if it's a TCP packet.
    /// </summary>
    public static uint? GetTcpAcknowledgmentNumber(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        return tcpPacket?.AcknowledgmentNumber;
    }

    /// <summary>
    /// Gets the TCP window size if it's a TCP packet.
    /// </summary>
    public static ushort? GetTcpWindowSize(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        return tcpPacket?.WindowSize;
    }

    /// <summary>
    /// Determines if the packet is an IGMP packet.
    /// </summary>
    public static bool IsIgmpPacket(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<IgmpPacket>() != null;
    }

    /// <summary>
    /// Gets the Time To Live (TTL) value from the IP packet.
    /// </summary>
    public static int? GetTimeToLive(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        return ipPacket?.TimeToLive;
    }

    /// <summary>
    /// Determines if the packet is part of a TCP handshake.
    /// </summary>
    public static bool IsTcpHandshake(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            return tcpPacket.Synchronize && !tcpPacket.Acknowledgment;
        }
        return false;
    }

    /// <summary>
    /// Determines if the packet is an Ethernet packet.
    /// </summary>
    public static bool IsEthernetPacket(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<EthernetPacket>() != null;
    }

    /// <summary>
    /// Gets the Ethernet type of the packet.
    /// </summary>
    public static EthernetPacket? GetEthernetType(this DevicePacket devicePacket)
    {
        var ethernetPacket = devicePacket.Packet.Extract<EthernetPacket>();
        return ethernetPacket;
    }

    /// <summary>
    /// Determines if the packet is a multicast packet.
    /// </summary>
    public static bool IsMulticast(this DevicePacket devicePacket)
    {
        var ethernetPacket = devicePacket.Packet.Extract<EthernetPacket>();
        if (ethernetPacket != null)
        {
            var firstByte = ethernetPacket.DestinationHardwareAddress.GetAddressBytes()[0];
            return (firstByte & 0x01) != 0;
        }
        return false;
    }

    /// <summary>
    /// Gets the total length of the IP packet.
    /// </summary>
    public static int? GetIpTotalLength(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        return ipPacket?.TotalLength;
    }

    /// <summary>
    /// Determines if the packet is a DHCP packet.
    /// </summary>
    public static bool IsDhcpPacket(this DevicePacket devicePacket)
    {
        var udpPacket = devicePacket.Packet.Extract<UdpPacket>();
        if (udpPacket != null)
        {
            int srcPort = udpPacket.SourcePort;
            int dstPort = udpPacket.DestinationPort;
            return srcPort == 67 || dstPort == 67 || srcPort == 68 || dstPort == 68;
        }
        return false;
    }
}
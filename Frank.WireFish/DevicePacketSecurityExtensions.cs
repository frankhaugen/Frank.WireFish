using System.Net;
using System.Text;

using PacketDotNet;

using SharpPcap;

namespace Frank.WireFish;

/// <summary>
/// Provides security-focused extension methods for the DevicePacket record.
/// </summary>
public static class DevicePacketSecurityExtensions
{
    /// <summary>
    /// Determines if the TCP packet has no flags set (NULL scan), which is a type of port scan.
    /// </summary>
    public static bool IsTcpNullScan(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            return !tcpPacket.Synchronize && !tcpPacket.Acknowledgment &&
                   !tcpPacket.Finished && !tcpPacket.Reset &&
                   !tcpPacket.Push && !tcpPacket.Urgent;
        }
        return false;
    }

    /// <summary>
    /// Determines if the TCP packet is a FIN scan (only FIN flag is set).
    /// </summary>
    public static bool IsTcpFinScan(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            return tcpPacket.Finished && !tcpPacket.Synchronize && !tcpPacket.Acknowledgment &&
                   !tcpPacket.Reset && !tcpPacket.Push && !tcpPacket.Urgent;
        }
        return false;
    }

    /// <summary>
    /// Determines if the TCP packet is an Xmas scan (FIN, PSH, and URG flags are set).
    /// </summary>
    public static bool IsTcpXmasScan(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            return tcpPacket.Finished && tcpPacket.Push && tcpPacket.Urgent &&
                   !tcpPacket.Synchronize && !tcpPacket.Acknowledgment && !tcpPacket.Reset;
        }
        return false;
    }

    /// <summary>
    /// Checks if the packet is malformed, which may indicate a potential attack or corruption.
    /// </summary>
    public static bool IsMalformedPacket(this DevicePacket devicePacket)
    {
        try
        {
            var ipPacket = devicePacket.Packet.Extract<IPPacket>();
            if (ipPacket != null)
            {
                // Total length should be greater than or equal to header length
                if (ipPacket.TotalLength < ipPacket.HeaderLength)
                    return true;

                // Payload length should match total length minus header length
                if (ipPacket.PayloadData.Length != ipPacket.TotalLength - ipPacket.HeaderLength)
                    return true;
            }
            return false;
        }
        catch
        {
            // Any exception during parsing indicates a malformed packet
            return true;
        }
    }

    /// <summary>
    /// Checks if the packet contains a payload that matches known malicious patterns.
    /// </summary>
    public static bool ContainsSuspiciousPayload(this DevicePacket devicePacket)
    {
        var payload = devicePacket.Packet.PayloadData;
        if (payload != null && payload.Length > 0)
        {
            // Example patterns (simplified for demonstration)
            var suspiciousPatterns = new byte[][]
            {
                Encoding.ASCII.GetBytes("malicious"),
                Encoding.ASCII.GetBytes("exploit"),
                new byte[] { 0x90, 0x90, 0x90 } // NOP sled
            };

            foreach (var pattern in suspiciousPatterns)
            {
                if (ContainsPattern(payload, pattern))
                    return true;
            }
        }
        return false;
    }

    private static bool ContainsPattern(byte[] data, byte[] pattern)
    {
        for (int i = 0; i <= data.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (data[i + j] != pattern[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines if the packet is an IP spoofing attempt based on known invalid source addresses.
    /// </summary>
    public static bool IsIpSpoofed(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        if (ipPacket != null)
        {
            var sourceIp = ipPacket.SourceAddress;

            // List of known invalid source IP addresses
            var invalidSources = new List<IPAddress>
            {
                IPAddress.Parse("0.0.0.0"),
                IPAddress.Broadcast,
                IPAddress.Loopback,
                IPAddress.Parse("255.255.255.255")
            };

            if (invalidSources.Contains(sourceIp) || IsPrivateIpAddressOnPublicInterface(sourceIp, devicePacket.Device))
                return true;
        }
        return false;
    }

    private static bool IsPrivateIpAddressOnPublicInterface(IPAddress ipAddress, ICaptureDevice device)
    {
        // Simplified check for private IP ranges
        var bytes = ipAddress.GetAddressBytes();
        switch (bytes[0])
        {
            case 10:
            case 172 when bytes[1] >= 16 && bytes[1] <= 31:
            case 192 when bytes[1] == 168:
                // Check if the interface is public (simplified, as actual implementation would require more context)
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Checks if the packet is an oversized ICMP packet, which may be used in Ping of Death attacks.
    /// </summary>
    public static bool IsOversizedIcmpPacket(this DevicePacket devicePacket)
    {
        var icmpv4Packet = devicePacket.Packet.Extract<IcmpV4Packet>();
        if (icmpv4Packet != null)
        {
            // Standard maximum size for an IP packet is 65535 bytes
            return devicePacket.Packet.TotalPacketLength > 65535;
        }
        
        var icmpv6Packet = devicePacket.Packet.Extract<IcmpV6Packet>();
        if (icmpv6Packet != null)
        {
            // Standard maximum size for an IPv6 packet is 65575 bytes
            return devicePacket.Packet.TotalPacketLength > 65575;
        }
        
        return false;
    }

    /// <summary>
    /// Determines if the packet is attempting a Land Attack (source and destination IP and ports are the same).
    /// </summary>
    public static bool IsLandAttack(this DevicePacket devicePacket)
    {
        var ipPacket = devicePacket.Packet.Extract<IPPacket>();
        if (ipPacket != null && ipPacket.SourceAddress.Equals(ipPacket.DestinationAddress))
        {
            var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
            if (tcpPacket != null && tcpPacket.SourcePort == tcpPacket.DestinationPort)
            {
                return true;
            }
            var udpPacket = devicePacket.Packet.Extract<UdpPacket>();
            if (udpPacket != null && udpPacket.SourcePort == udpPacket.DestinationPort)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks for unusual TCP option lengths which may indicate a potential attack.
    /// </summary>
    public static bool HasSuspiciousTcpOptions(this DevicePacket devicePacket)
    {
        return devicePacket.Packet.Extract<TcpPacket>() is { Options.Length: > 40 };
    }

    /// <summary>
    /// Checks if the packet contains a potential buffer overflow attempt in the payload.
    /// </summary>
    public static bool IsBufferOverflowAttempt(this DevicePacket devicePacket)
    {
        var payload = devicePacket.Packet.PayloadData;
        if (payload != null && payload.Length > 1000)
        {
            // Simplified check: repetitive characters
            int repetitiveThreshold = 1000;
            byte firstByte = payload[0];
            int count = 1;

            for (int i = 1; i < payload.Length; i++)
            {
                if (payload[i] == firstByte)
                {
                    count++;
                    if (count >= repetitiveThreshold)
                        return true;
                }
                else
                {
                    count = 1;
                    firstByte = payload[i];
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the packet is using deprecated or insecure protocols.
    /// </summary>
    public static bool IsUsingInsecureProtocol(this DevicePacket devicePacket)
    {
        var tcpPacket = devicePacket.Packet.Extract<TcpPacket>();
        if (tcpPacket != null)
        {
            int[] insecurePorts = { 23, 21, 69 }; // Telnet, FTP, TFTP
            if (Array.Exists(insecurePorts, port => port == tcpPacket.SourcePort || port == tcpPacket.DestinationPort))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the packet contains an attempt at SQL injection.
    /// </summary>
    public static bool ContainsSqlInjectionAttempt(this DevicePacket devicePacket)
    {
        var payload = devicePacket.Packet.PayloadData;
        if (payload != null && payload.Length > 0)
        {
            string payloadString = Encoding.UTF8.GetString(payload);
            string[] sqlInjectionPatterns = { "' OR '1'='1", "'; DROP TABLE", "--", "/*", "*/", "@@", "char", "nchar", "varchar", "nvarchar" };
            foreach (var pattern in sqlInjectionPatterns)
            {
                if (payloadString.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the packet is an attempt at a Ping of Death attack.
    /// </summary>
    public static bool IsPingOfDeathAttempt(this DevicePacket devicePacket)
    {
        var icmpV4Packet = devicePacket.Packet.Extract<IcmpV4Packet>();
        if (icmpV4Packet != null)
        {
            return devicePacket.Packet.TotalPacketLength > 65535;
        }
        
        var icmpV6Packet = devicePacket.Packet.Extract<IcmpV6Packet>();
        if (icmpV6Packet != null)
        {
            return devicePacket.Packet.TotalPacketLength > 65575;
        }
        
        return false;
    }
}
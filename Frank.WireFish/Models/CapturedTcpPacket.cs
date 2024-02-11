using PacketDotNet;

namespace Frank.WireFish.Models;

public record CapturedTcpPacket(DeviceInfo DeviceInfo, IpPort Source, IpPort Destination, Packet Packet, IPPacket IpPacket, TcpPacket TcpPacket);
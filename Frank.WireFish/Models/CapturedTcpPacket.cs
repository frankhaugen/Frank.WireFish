using PacketDotNet;

namespace Frank.WireFish;

public record CapturedTcpPacket(IpPort Source, IpPort Destination, Packet Packet, IPPacket IpPacket, TcpPacket TcpPacket);
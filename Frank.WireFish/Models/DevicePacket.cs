using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish.Models;

public record DevicePacket(ICaptureDevice Device, Packet Packet, DateTime Timestamp);
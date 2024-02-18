using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish;

/// <summary>
/// Represents a device packet containing information about the captured packet.
/// </summary>
public record DevicePacket(ICaptureDevice Device, Packet Packet, DateTime Timestamp);
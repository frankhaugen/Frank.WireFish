using PacketDotNet;
using SharpPcap;

namespace Frank.WireFish.Models;

public record IrcDevicePacket(ICaptureDevice Device, Packet Packet, DateTime Timestamp) : DevicePacket(Device, Packet, Timestamp);
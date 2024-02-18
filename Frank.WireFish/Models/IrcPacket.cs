namespace Frank.WireFish.Models;

public record IrcPacket(DateTime Timestamp, string Source, string Destination, string? Message);
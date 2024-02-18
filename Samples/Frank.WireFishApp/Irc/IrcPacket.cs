namespace Frank.WireFishApp.Irc;

public record IrcPacket(DateTime Timestamp, string Source, string Destination, string Device, string? Message);
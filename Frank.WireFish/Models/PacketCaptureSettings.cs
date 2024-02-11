namespace Frank.WireFish.Models;

public class PacketCaptureSettings
{
    // public string Device { get; set; } = @"\Device\NPF_Loopback";
    public string Filter { get; set; } = "tcp";
}
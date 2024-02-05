using System.Net.NetworkInformation;
using SharpPcap;

namespace Frank.WireFish;

public class CaptureWrapper
{
    public string DeviceName { get; set; }
    
    public RawCapture Capture { get; set; }
    
    public ICaptureDevice Device { get; set; }
    
    public ICaptureHeader Header { get; set; }
    public bool? Inbound { get; set; }
}
using SharpPcap;

namespace Frank.WireFish;

public class CaptureWrapper
{
    public RawCapture Capture { get; set; }
    
    public ICaptureDevice Device { get; set; }
    
    public ICaptureHeader Header { get; set; }
}
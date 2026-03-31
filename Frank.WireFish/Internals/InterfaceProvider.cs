using System.Net.NetworkInformation;

namespace Frank.WireFish.Internals;

internal class InterfaceProvider
{
    private readonly IEnumerable<NetworkInterface> _interfaces;
    
    public InterfaceProvider()
    {
        _interfaces = NetworkInterface.GetAllNetworkInterfaces();
    }
    
    public IEnumerable<NetworkInterface> GetInterfaces() => _interfaces;
}
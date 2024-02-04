namespace Frank.WireFish;

public interface IAsyncLifetime : IAsyncDisposable, IDisposable
{
    Task StartAsync();
    
    Task StopAsync();
}
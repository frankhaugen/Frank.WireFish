using Frank.WireFish;

namespace Frank.WireFishApp;

public class CaptureDataRunner : IHostedService
{
    private readonly IPacketCaptureService _packetCaptureService;
    private readonly ILogger<CaptureDataRunner> _logger;

    public CaptureDataRunner(IPacketCaptureService packetCaptureService, ILogger<CaptureDataRunner> logger)
    {
        _packetCaptureService = packetCaptureService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting packet capture service");
        await _packetCaptureService.StartAsync();
        _logger.LogInformation("Packet capture service started");
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping packet capture service");
        await _packetCaptureService.StopAsync();
        _logger.LogInformation("Packet capture service stopped");
    }
}


namespace Frank.WireFish;

public interface IPacketHandler
{
    Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken);
    
    bool CanHandle(DevicePacket packet);
}
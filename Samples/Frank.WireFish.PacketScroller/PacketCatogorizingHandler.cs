namespace Frank.WireFish.PacketScroller;

public class PacketCatogorizingHandler : IPacketHandler
{
    private readonly IPacketContainer _packetContainer;

    public PacketCatogorizingHandler(IPacketContainer packetContainer)
    {
        _packetContainer = packetContainer;
    }

    /// <inheritdoc />
    public async Task HandleAsync(DevicePacket packet, CancellationToken cancellationToken)
    {
        _packetContainer.Add(packet);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool CanHandle(DevicePacket packet) => true;
}
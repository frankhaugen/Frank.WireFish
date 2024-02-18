namespace Frank.WireFish;

public interface IWireFishBuilder
{
    IWireFishBuilder AddPacketHandler<THandler>() where THandler : class, IPacketHandler;
}
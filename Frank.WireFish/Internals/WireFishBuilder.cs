using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish.Internals;

internal class WireFishBuilder(IServiceCollection services) : IWireFishBuilder
{
    /// <inheritdoc />
    public IWireFishBuilder AddPacketHandler<THandler>() where THandler : class, IPacketHandler
    {
        services.AddSingleton<IPacketHandler, THandler>();
        return this;
    }
}
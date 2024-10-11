using Microsoft.Extensions.DependencyInjection;

namespace Frank.WireFish.Internals;

internal class WireFishBuilder(IServiceCollection services) : IWireFishBuilder
{
    /// <inheritdoc />
    public IWireFishBuilder AddPacketHandler<THandler>() where THandler : class, IPacketHandler
    {
        if (services.All(x => x.ServiceType != typeof(THandler))) 
            services.AddSingleton<IPacketHandler, THandler>();
        return this;
    }
}
using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public static class RealmServiceExtensions
{
    public static IServiceCollection AddRealmService(this IServiceCollection services, RealmConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddSingleton<IRealmService, RealmService>();
        return services;
    }
    
    public static IServiceCollection AddRepository<T>(this IServiceCollection services) where T : RealmObject
    {
        services.AddSingleton<IRepository<T>, RealmRepository<T>>();
        return services;
    }
}
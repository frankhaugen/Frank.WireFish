using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public class RealmRepository<T> : IRepository<T> where T : RealmObject
{
    private readonly IRealmService _realmService;

    public RealmRepository(IRealmService realmService)
    {
        _realmService = realmService;
    }

    public async Task AddAsync(T entity)
    {
        await _realmService.AddAsync(entity);
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        return await _realmService.GetAllAsync<T>();
    }

    public async Task<IQueryable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> query)
    {
        return await _realmService.QueryAsync(query);
    }

    public async Task UpdateAsync(string primaryKey, Action<T> updateAction)
    {
        await _realmService.UpdateAsync(updateAction, primaryKey);
    }

    public async Task RemoveAsync(T entity)
    {
        await _realmService.RemoveAsync(entity);
    }
}
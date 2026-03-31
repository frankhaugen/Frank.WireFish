using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public interface IRealmService
{
    Task AddAsync<T>(T entity) where T : RealmObject;
    Task<IQueryable<T>> GetAllAsync<T>() where T : RealmObject;
    Task<IQueryable<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>> query) where T : RealmObject;
    Task UpdateAsync<T>(Action<T> updateAction, string primaryKey) where T : RealmObject;
    Task RemoveAsync<T>(T entity) where T : RealmObject;
}
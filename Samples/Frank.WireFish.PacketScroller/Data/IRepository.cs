using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public interface IRepository<T> where T : RealmObject
{
    Task AddAsync(T entity);
    Task<IQueryable<T>> GetAllAsync();
    Task<IQueryable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> query);
    Task UpdateAsync(string primaryKey, Action<T> updateAction);
    Task RemoveAsync(T entity);
}
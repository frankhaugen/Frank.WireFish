using Realms;

namespace Frank.WireFish.PacketScroller.Data;

public class RealmService : IRealmService
{
    private readonly RealmConfiguration? _config;

    public RealmService(RealmConfiguration config)
    {
        _config = config;
    }

    public async Task AddAsync<T>(T entity) where T : RealmObject
    {
        await Task.Run(() =>
        {
            using var realm = Realm.GetInstance(_config);
            realm.Write(() =>
            {
                realm.Add(entity);
            });
        });
    }

    public async Task<IQueryable<T>> GetAllAsync<T>() where T : RealmObject
    {
        return await Task.Run(() =>
        {
            using var realm = Realm.GetInstance(_config);
            return realm.All<T>();
        });
    }

    public async Task<IQueryable<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>> query) where T : RealmObject
    {
        return await Task.Run(() =>
        {
            using var realm = Realm.GetInstance(_config);
            var results = query(realm.All<T>());
            return results;
        });
    }

    public async Task UpdateAsync<T>(Action<T> updateAction, string primaryKey) where T : RealmObject
    {
        await Task.Run(() =>
        {
            using var realm = Realm.GetInstance(_config);
            var entity = realm.Find<T>(primaryKey);
            if (entity != null)
            {
                realm.Write(() =>
                {
                    updateAction(entity);
                });
            }
        });
    }

    public async Task RemoveAsync<T>(T entity) where T : RealmObject
    {
        await Task.Run(() =>
        {
            using var realm = Realm.GetInstance(_config);
            realm.Write(() =>
            {
                realm.Remove(entity);
            });
        });
    }
}
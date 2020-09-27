using System;
using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.Caching.Contracts
{
    public interface IRedisCacheService
    {
        Task StoreInCacheAsync<T>(string key, T value);
        Task StoreInCacheAsync<T>(string key, T value, TimeSpan duration);
        Task<T> GetOrAddCachedAsync<T>(string key, Func<Task<T>> initializeFunction);
        Task<bool> Invalidate(string key);
    }
}
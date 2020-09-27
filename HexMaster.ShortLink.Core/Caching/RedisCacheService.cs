using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Caching.Contracts;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HexMaster.ShortLink.Core.Caching
{
    public sealed class RedisCacheService:IRedisCacheService
    {
                private IConnectionMultiplexer _connection;
        private IDatabase _database;
        private readonly string _connectionString;

        public RedisCacheService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task StoreInCacheAsync<T>(string key, T value)
        {
            await StoreInCacheAsync(key, value, TimeSpan.FromMinutes(20));
        }

        public async Task StoreInCacheAsync<T>(string key, T value, TimeSpan duration)
        {
            await Connect();
            if (value == null)
            {
                return;
            }
            var cacheData = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, cacheData, duration);
        }

        public async Task<T> GetOrAddCachedAsync<T>(string key, Func<Task<T>> initializeFunction)
        {
            T model = default;
            bool cacheIsAvailable;
            try
            {
                await Connect();
                var cacheData = await _database.StringGetAsync(key);
                if (!string.IsNullOrWhiteSpace(cacheData))
                {
                    model = JsonConvert.DeserializeObject<T>(cacheData);
                }

                cacheIsAvailable = true;
            }
            catch (Exception ex)
            {
//                _logger.LogWarning(ex,"The cache service appears to be not working. Could not connect or fetch value from cache");
                cacheIsAvailable = false;
            }

            if (model == null && initializeFunction!=null)
            {
                model = await initializeFunction();
                if (cacheIsAvailable)
                {
                    await StoreInCacheAsync(key, model);
                }
            }

            return model;
        }

        public async Task<bool> Invalidate(string key)
        {
            await Connect();
            return await _database.KeyDeleteAsync(key);
        }

        private async Task Connect()
        {
            if (_database == null)
            {
                _connection = await ConnectionMultiplexer.ConnectAsync(_connectionString);
                _database = _connection.GetDatabase();
            }
        }
    }
}

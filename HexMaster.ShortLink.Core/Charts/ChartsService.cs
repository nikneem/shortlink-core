using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Caching.Contracts;
using HexMaster.ShortLink.Core.Charts.Contracts;
using HexMaster.ShortLink.Core.Charts.Models;

namespace HexMaster.ShortLink.Core.Charts
{
    public sealed class ChartsService : IChartsService
    {
        private readonly IChartsRepository _repository;
        private readonly IRedisCacheServiceFactory _redisCacheFactory;

        public ChartsService(IChartsRepository repository, IRedisCacheServiceFactory redisCacheFactory)
        {
            _repository = repository;
            _redisCacheFactory = redisCacheFactory;
        }


        public async Task<List<HourlyHitsDto>> GetHourlyChartsAsync(string shortCode)
        {
            return await GetHourlyHitsChartsFromCacheOrRepositoryAsync(shortCode);
        }
        public async Task<List<DailyHitsDto>> GetDailyChartsAsync(string shortCode)
        {
            return await GetDailyHitsChartsFromCacheOrRepositoryAsync(shortCode);
        }
        public async Task<List<HourlyHitsDto>> GetSparkChartsAsync(string shortCode)
        {
            return await GetHourlyHitsChartsFromCacheOrRepositoryAsync(shortCode);
        }



        private async Task<List<HourlyHitsDto>> GetHourlyHitsChartsFromCacheOrRepositoryAsync(string shortCode)
        {
            var cacheKey = $"HourlyHitsPerShortCode-{shortCode}";
            var cache =  _redisCacheFactory.Connect();
            return await cache.GetOrAddCachedAsync(cacheKey, () => GetHourlyHitsChartsFromRepositoryAsync(shortCode));
        }
        private async Task<List<HourlyHitsDto>> GetHourlyHitsChartsFromRepositoryAsync(string shortCode)
        {
            return await _repository.GetHourlyChartAsync(shortCode);
        }


        private async Task<List<DailyHitsDto>> GetDailyHitsChartsFromCacheOrRepositoryAsync(string shortCode)
        {
            var cacheKey = $"DailyHitsPerShortCode-{shortCode}";
            var cache =  _redisCacheFactory.Connect();
            return await cache.GetOrAddCachedAsync(cacheKey, () => GetDailyHitsChartsFromRepositoryAsync(shortCode));
        }
        private async Task<List<DailyHitsDto>> GetDailyHitsChartsFromRepositoryAsync(string shortCode)
        {
            return await _repository.GetDailyChartAsync(shortCode);
        }


        private async Task<List<HourlyHitsDto>> GetSparkChartsFromCacheOrRepositoryAsync(string shortCode)
        {
            var cacheKey = $"SparkLinePerShortCode-{shortCode}";
            var cache =  _redisCacheFactory.Connect();
            return await cache.GetOrAddCachedAsync(cacheKey, () => GetSparkChartsFromRepositoryAsync(shortCode));
        }
        private async Task<List<HourlyHitsDto>> GetSparkChartsFromRepositoryAsync(string shortCode)
        {
            return await _repository.GetHourlyChartAsync(shortCode, 12);
        }

    }
}

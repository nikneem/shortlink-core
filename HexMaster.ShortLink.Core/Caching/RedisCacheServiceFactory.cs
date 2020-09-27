using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Caching.Contracts;
using HexMaster.ShortLink.Core.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace HexMaster.ShortLink.Core.Caching
{
    public sealed class RedisCacheServiceFactory : IRedisCacheServiceFactory
    {
        private readonly IOptions<CloudConfiguration> _configuration;

        public RedisCacheServiceFactory(IOptions<CloudConfiguration> configuration)
        {
            _configuration = configuration;
        }


        public IRedisCacheService Connect()
        {
            return new RedisCacheService(_configuration.Value.RedisCacheConnectionString);
        }

    }
}

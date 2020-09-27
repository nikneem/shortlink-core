namespace HexMaster.ShortLink.Core.Caching.Contracts
{
    public interface IRedisCacheServiceFactory
    {
        IRedisCacheService Connect();
    }
}


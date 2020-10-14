using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.Hits.Contracts
{
    public interface IHitsRepository
    {
        Task CreateNewAsync(DateTimeOffset eventDate, string shortCode);
        Task<List<Tuple<string, int>>> GetHitsPerShortCode();
    }
}
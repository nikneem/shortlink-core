using System;
using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.Hits.Contracts
{
    public interface IHitsRepository
    {
        Task CreateNewAsync(DateTimeOffset eventDate, string shortCode);
    }
}
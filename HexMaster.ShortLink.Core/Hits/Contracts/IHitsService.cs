using System;
using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.Hits.Contracts
{
    public interface IHitsService
    {
        Task RegisterHitAsync(string shortCode, DateTimeOffset eventDate);
    }
}
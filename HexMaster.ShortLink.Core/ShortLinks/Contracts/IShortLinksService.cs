using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Models;

namespace HexMaster.ShortLink.Core.Contracts
{
    public interface IShortLinksService
    {
        Task<string> GenerateUniqueShortLink();
        Task<ShortLinkDetailsDto> CreateAsync(ShortLinkCreateDto dto);
        Task UpdateAsync(Guid id, ShortLinkUpdateDto dto);

    }
}
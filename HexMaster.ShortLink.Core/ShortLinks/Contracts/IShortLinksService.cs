using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Models;

namespace HexMaster.ShortLink.Core.Contracts
{
    public interface IShortLinksService
    {
        Task<string> GenerateUniqueShortLink();
        Task<List<ShortLinkListItemDto>> ListAsync(string ownerSubjectId);
        Task<ShortLinkDetailsDto> CreateAsync(ShortLinkCreateDto dto, string ownerSubjectId);
        Task<ShortLinkDetailsDto> ReadAsync(Guid id, string ownerSubjectId);
        Task UpdateAsync(Guid id, ShortLinkUpdateDto dto, string ownerSubjectId);
        Task DeleteAsync(Guid id, string ownerSubjectId);

    }
}
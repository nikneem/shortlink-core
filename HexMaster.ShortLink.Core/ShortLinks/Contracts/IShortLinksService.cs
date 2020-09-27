using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Models;

namespace HexMaster.ShortLink.Core.Contracts
{
    public interface IShortLinksService
    {
        Task<string> GenerateUniqueShortLink();
        Task<List<ShortLinkListItemDto>> ListAsync(string ownerSubjectId, int page, int pageSize);
        Task<ShortLinkDetailsDto> CreateAsync(string ownerSubjectId, ShortLinkCreateDto dto);
        Task<ShortLinkDetailsDto> ReadAsync(string ownerSubjectId, Guid id);
        Task UpdateAsync(string ownerSubjectId, Guid id, ShortLinkUpdateDto dto);
        Task DeleteAsync(string ownerSubjectId, Guid id);

        Task<string> Resolve(string code);

    }
}
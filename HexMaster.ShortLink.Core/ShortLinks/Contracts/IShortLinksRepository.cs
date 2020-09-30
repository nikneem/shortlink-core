using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Models;

namespace HexMaster.ShortLink.Core.Contracts
{
    public interface IShortLinksRepository
    {

        Task<List<ShortLinkListItemDto>> GetShortLinksListAsync(string ownerId, int page, int pageSize);

        Task<ShortLinkDetailsDto> GetShortLinkDetailsAsync(string ownerId, Guid id);

        Task<ShortLinkDetailsDto> CreateNewShortLinkAsync(
            string shortCode,
            string endpointUrl,
            string ownerId);

        Task UpdateExistingShortLinkAsync(string ownerId, ShortLinkUpdateDto dto);

        Task DeleteShortLinkAsync(string ownerId, Guid id);

        Task IncreaseHitsAsync(string shortCode);
        
        Task<bool> CheckIfShortCodeIsUniqueAsync(string shortCode);
        
        Task<bool> CheckIfShortCodeIsUniqueForShortLinkAsync(Guid id, string shortCode);
        
        Task<string> ResolveAsync(string shortCode);

    }
}
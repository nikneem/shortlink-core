using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Models;

namespace HexMaster.ShortLink.Core.Contracts
{
    public interface IShortLinksRepository
    {
        Task<bool> CheckIfShortCodeIsUniqueAsync(string shortCode);
        Task<bool> CheckIfShortCodeIsUniqueForShortLinkAsync(Guid id, string shortCode);

        Task<ShortLinkDetailsDto> CreateNewShortLinkAsync(
            string shortCode,
            string endpointUrl,
            string ownerId);

        Task UpdateExistingShortLinkAsync(ShortLinkUpdateDto dto);
    }
}
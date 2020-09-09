using System.Threading.Tasks;
using HexMaster.ShortLink.Core.ShortLinks.Models;

namespace HexMaster.ShortLink.Core.ShortLinks.Contracts
{
    public interface IShortLinksRepository
    {
        Task<bool> CheckIfShortCodeIsUniqueAsync(string shortCode);

        Task<ShortLinkDetailsDto> CreateNewShortLinkAsync(
            string shortCode,
            string endpointUrl,
            string ownerId);
    }
}
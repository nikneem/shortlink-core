using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.ShortLinks.Contracts
{
    public interface IShortLinksRepository
    {
        Task<bool> CheckIfShortCodeIsUniqueAsync(string shortCode);
    }
}
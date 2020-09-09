using System.Threading.Tasks;

namespace HexMaster.ShortLink.Core.ShortLinks.Contracts
{
    public interface IShortLinksService
    {
        Task<string> GenerateUniqueShortLink();
    }
}
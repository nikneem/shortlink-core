using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Helpers
{
    public interface ICloudTableFactory
    {
        Task<CloudTable> GetCloudTableReferenceAsync(string name);
    }
}
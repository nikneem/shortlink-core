using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Configuration;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace HexMaster.ShortLink.Core.Helpers
{
    public class CloudTableFactory : ICloudTableFactory
    {
        private readonly IOptions<CloudConfiguration> _configuration;
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;

        public CloudTableFactory(IOptions<CloudConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public async Task<CloudTable> GetCloudTableReferenceAsync(string name)
        {
            var tableClient = GetCloudTableClient();
            var cloudTable = tableClient.GetTableReference(name);
            await cloudTable.CreateIfNotExistsAsync();
            return cloudTable;
        }


        private CloudTableClient GetCloudTableClient()
        {
            _tableClient ??= GetStorageAccount().CreateCloudTableClient();
            return _tableClient;
        }

        private CloudStorageAccount GetStorageAccount()
        {
            _storageAccount ??= CloudStorageAccount.Parse(_configuration.Value.StorageConnectionString);
            return _storageAccount;
        }

    }
}
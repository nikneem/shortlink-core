using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Entities;
using HexMaster.ShortLink.Core.Helpers;
using HexMaster.ShortLink.Core.Hits.Contracts;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Hits
{
    public sealed class HitsRepository : IHitsRepository
    {
        private readonly ICloudTableFactory _cloudTableFactory;


        public async Task CreateNewAsync(DateTimeOffset eventDate, string shortCode)
        {
            var table = await _cloudTableFactory.GetCloudTableReferenceAsync(TableNames.Hits);

            var entity = new HitEntity
            {
                PartitionKey = PartitionKeys.Hit,
                RowKey = Guid.NewGuid().ToString(),
                CreatedOn = eventDate,
                ShortCode = shortCode,
                Timestamp = DateTimeOffset.UtcNow
            };

            var operation = TableOperation.Insert(entity);
            await table.ExecuteAsync(operation);
        }

        public HitsRepository(ICloudTableFactory cloudTableFactory)
        {
            _cloudTableFactory = cloudTableFactory;
        }


    }
}

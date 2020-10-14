using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Tuple<string, int>>> GetHitsPerShortCode()
        {
            var hitsList = new List<Tuple<string, int>>();
            var table = await _cloudTableFactory.GetCloudTableReferenceAsync(TableNames.Hits);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(HitEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.Hit);

            var query = new TableQuery<HitEntity>().Where(partitionKeyFilter);
            var ct = new TableContinuationToken();

            var hitEntities = new List<HitEntity>();
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, ct);
                hitEntities.AddRange(segment.Results);
                ct = segment.ContinuationToken;
            } while (ct != null);

            var summedHits = hitEntities.GroupBy(h => h.ShortCode).Select(
                grp => new 
                {
                    ShortCode = grp.Key,
                    Count = grp.Count()
                });
            foreach (var sh in summedHits)
            {
                hitsList.Add(new Tuple<string, int>(sh.ShortCode, sh.Count));
            }

            return hitsList;
        }

        public HitsRepository(ICloudTableFactory cloudTableFactory)
        {
            _cloudTableFactory = cloudTableFactory;
        }


    }
}

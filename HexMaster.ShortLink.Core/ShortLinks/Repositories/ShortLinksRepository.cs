using System;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Entities;
using HexMaster.ShortLink.Core.Helpers;
using HexMaster.ShortLink.Core.Models;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Repositories
{
    public class ShortLinksRepository : IShortLinksRepository
    {
        private readonly ICloudTableFactory _tableFactory;

        public ShortLinksRepository(ICloudTableFactory tableFactory)
        {
            _tableFactory = tableFactory;
        }

        public async Task<bool> CheckIfShortCodeIsUniqueAsync(string shortCode)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var shortCodeFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.ShortCode),
                QueryComparisons.Equal,
                shortCode);

            var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, shortCodeFilter);
            var query = new TableQuery<ShortLinkEntity>().Where(queryFilter);
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            return segment.Results.Count == 0;
        }

        public async Task<bool> CheckIfShortCodeIsUniqueForShortLinkAsync(Guid id, string shortCode)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var shortCodeFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.ShortCode),
                QueryComparisons.Equal,
                shortCode);
            var idFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.RowKey),
                QueryComparisons.NotEqual,
                id.ToString());

            var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, TableQuery.CombineFilters(shortCodeFilter, TableOperators.And, idFilter));
            var query = new TableQuery<ShortLinkEntity>().Where(queryFilter);
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            return segment.Results.Count == 0;
        }

        public async Task<ShortLinkDetailsDto> CreateNewShortLinkAsync(
            string shortCode, 
            string endpointUrl,
            string ownerId)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);
            var shortLinkEntity = new ShortLinkEntity
            {
                PartitionKey = PartitionKeys.ShortLinks,
                RowKey = Guid.NewGuid().ToString(),
                EndpointUrl = endpointUrl,
                ShortCode = shortCode,
                CreatedOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMonths(3),
                OwnerId = ownerId,
                TotalHits = 0,
                Timestamp = DateTimeOffset.UtcNow
            };
            var tableOperation = TableOperation.Insert(shortLinkEntity);
            await table.ExecuteAsync(tableOperation);
            return ShortLinkDetailsDto.CreateFromEntity(shortLinkEntity);
        }

        public async Task UpdateExistingShortLinkAsync(ShortLinkUpdateDto dto)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);
            var entity = new DynamicTableEntity(PartitionKeys.ShortLinks, dto.Id.ToString()) {ETag = "*"};

            entity.Properties.Add(nameof(ShortLinkEntity.ShortCode), new EntityProperty(dto.ShortCode));
            entity.Properties.Add(nameof(ShortLinkEntity.EndpointUrl), new EntityProperty(dto.EndpointUrl));
            entity.Properties.Add(nameof(ShortLinkEntity.ExpiresOn), new EntityProperty(dto.ExpirationOn));

            var mergeOperation = TableOperation.Merge(entity);
            await table.ExecuteAsync(mergeOperation);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Entities;
using HexMaster.ShortLink.Core.Exceptions;
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

        public async Task<List<ShortLinkListItemDto>> GetShortLinksListAsync(
            string ownerId, 
            int page,
            int pageSize)
        {
            var completeList = new List<ShortLinkListItemDto>();
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var ownerFilter = TableQuery.GenerateFilterCondition(
                nameof(ShortLinkEntity.OwnerId),
                QueryComparisons.Equal,
                ownerId);

            var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, ownerFilter);
            var query = new TableQuery<ShortLinkEntity>().Where(queryFilter);
            var ct = new TableContinuationToken();

            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync(query, ct);
                ct = segment.ContinuationToken;
                var models = segment.Results.Select(ent => new ShortLinkListItemDto
                {
                    Id = Guid.Parse(ent.RowKey),
                    ShortCode = ent.ShortCode,
                    TotalHits = ent.TotalHits,
                    ExpirationOn = ent.ExpiresOn,
                    EndpointUrl = ent.EndpointUrl
                });
                completeList.AddRange(models);
            } while (ct != null);

            if (page < 0)
            {
                page = 0;
            }

            if (pageSize < 10 || pageSize > 100)
            {
                pageSize = 25;
            }

            var skip = page * pageSize;

            return completeList
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }

        public async Task<ShortLinkDetailsDto> GetShortLinkDetailsAsync(string ownerId, Guid id)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var operation = TableOperation.Retrieve<ShortLinkEntity>(PartitionKeys.ShortLinks, id.ToString());
            var result = await table.ExecuteAsync(operation);
            if (result.Result is ShortLinkEntity entity)
            {
                if (entity.OwnerId != ownerId)
                {
                    throw new ShortLinkNotFoundException(id);
                }
                return new ShortLinkDetailsDto
                {
                    ShortCode = entity.ShortCode,
                    EndpointUrl = entity.EndpointUrl,
                    ExpirationOn = entity.ExpiresOn,
                    TotalHits = entity.TotalHits,
                    CreatedOn = entity.CreatedOn,
                    Id = Guid.Parse(entity.RowKey)
                };
            }

            throw new ShortLinkNotFoundException(id);
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

        public async Task UpdateExistingShortLinkAsync(string ownerId, ShortLinkUpdateDto dto)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);
            var operation = TableOperation.Retrieve<ShortLinkEntity>(PartitionKeys.ShortLinks, dto.Id.ToString());
            var result = await table.ExecuteAsync(operation);
            if (result.Result is ShortLinkEntity entity)
            {
                if (entity.OwnerId != ownerId)
                {
                    throw new ShortLinkNotFoundException(dto.Id);
                }

                entity.ShortCode = dto.ShortCode;
                entity.EndpointUrl = dto.EndpointUrl;
                entity.ExpiresOn = dto.ExpirationOn;

                var mergeOperation = TableOperation.InsertOrReplace(entity);
                await table.ExecuteAsync(mergeOperation);
            }
        }

        public async Task DeleteShortLinkAsync(string ownerId, Guid id)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var fetchOperation = TableOperation.Retrieve<ShortLinkEntity>(PartitionKeys.ShortLinks, id.ToString());
            var result = await table.ExecuteAsync(fetchOperation);
            if (result.Result is ShortLinkEntity entity)
            {
                if (entity.OwnerId != ownerId)
                {
                    throw new ShortLinkNotFoundException(id);
                }
                var operation = TableOperation.Delete(entity);
                await table.ExecuteAsync(operation);
            }
        }

        public async Task<string> Resolve(string shortCode)
        {
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.ShortLinks);

            var pkQuery = TableQuery.GenerateFilterCondition(nameof(ShortLinkEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var shortCodeQuery =
                TableQuery.GenerateFilterCondition(nameof(ShortLinkEntity.ShortCode),
                    QueryComparisons.Equal, shortCode);
            var expirationQuery = TableQuery.GenerateFilterConditionForDate(
                nameof(ShortLinkEntity.ExpiresOn),
                QueryComparisons.GreaterThanOrEqual, DateTimeOffset.UtcNow);

            var query = new TableQuery<ShortLinkEntity>().Where(TableQuery.CombineFilters(expirationQuery, TableOperators.And,TableQuery.CombineFilters(pkQuery, TableOperators.And,
                shortCodeQuery))).Take(1);
            var ct = new TableContinuationToken();
            var queryResult = await table.ExecuteQuerySegmentedAsync(query, ct);
            var entity = queryResult.Results.FirstOrDefault();
            return entity?.EndpointUrl;
        }
    }
}
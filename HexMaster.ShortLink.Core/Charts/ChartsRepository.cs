using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Charts.Contracts;
using HexMaster.ShortLink.Core.Charts.Models;
using HexMaster.ShortLink.Core.Entities;
using HexMaster.ShortLink.Core.Helpers;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Charts
{
    public sealed class ChartsRepository : IChartsRepository
    {
        private readonly ICloudTableFactory _tableFactory;

        public ChartsRepository(ICloudTableFactory tableFactory)
        {
            _tableFactory = tableFactory;
        }

        public async Task<List<HourlyHitsDto>> GetHourlyChartAsync(string shortCode, int hours = 24)
        {
            var startDate = DateTimeOffset.UtcNow.AddHours(-hours);
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.HitsPerHour);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(HitsAggregateHourlyEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var shortCodeFilter = TableQuery.GenerateFilterCondition(
                nameof(HitsAggregateHourlyEntity.ShortCode),
                QueryComparisons.Equal,
                shortCode);
            var dateFilter = TableQuery.GenerateFilterConditionForDate(
                nameof(HitsAggregateHourlyEntity.AggregateRangeStart),
                QueryComparisons.Equal,
                startDate);

            var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
                TableQuery.CombineFilters(dateFilter, TableOperators.And, shortCodeFilter));
            var query = new TableQuery<HitsAggregateHourlyEntity>().Where(queryFilter);
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            var list = segment.Results.Select(ent => new HourlyHitsDto
            {
                Start = ent.AggregateRangeStart,
                Hour = ent.AggregateRangeStart.ToString("HH:mm"),
                Hits = ent.TotalHits
            }).ToList();

            startDate = startDate.AddHours(1);
            do
            {
                var hourString = $"{startDate:HH}:00";
                if (!list.Any(x => x.Hour.Equals(hourString)))
                {
                    list.Add(new HourlyHitsDto
                    {
                        Start = startDate,
                        Hour = hourString,
                        Hits = 0
                    });
                }

                startDate = startDate.AddHours(1);
            } while (startDate < DateTimeOffset.UtcNow);

            return list.OrderBy(x => x.Start).ToList();
        }

        public async Task<List<DailyHitsDto>> GetDailyChartAsync(string shortCode, int days = 30)
        {
            var startDate = DateTimeOffset.UtcNow.AddDays(-days);
            var table = await _tableFactory.GetCloudTableReferenceAsync(TableNames.HitsPerDay);

            var partitionKeyFilter = TableQuery.GenerateFilterCondition(
                nameof(HitsAggregateHourlyEntity.PartitionKey),
                QueryComparisons.Equal,
                PartitionKeys.ShortLinks);
            var shortCodeFilter = TableQuery.GenerateFilterCondition(
                nameof(HitsAggregateHourlyEntity.ShortCode),
                QueryComparisons.Equal,
                shortCode);
            var dateFilter = TableQuery.GenerateFilterConditionForDate(
                nameof(HitsAggregateHourlyEntity.AggregateRangeStart),
                QueryComparisons.Equal,
                startDate);

            var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
                TableQuery.CombineFilters(dateFilter, TableOperators.And, shortCodeFilter));
            var query = new TableQuery<HitsAggregateHourlyEntity>().Where(queryFilter);
            var segment = await table.ExecuteQuerySegmentedAsync(query, null);

            var list = segment.Results.Select(ent => new DailyHitsDto
            {
                Start = ent.AggregateRangeStart,
                Hits = ent.TotalHits
            }).ToList();

            startDate = startDate.AddHours(1);
            do
            {
                if (!list.Any(ent => ent.Start.Day.Equals(startDate.Day) &&
                                     ent.Start.Month.Equals(startDate.Month) &&
                                     ent.Start.Year.Equals(startDate.Year)))
                {
                    list.Add(new DailyHitsDto
                    {
                        Hits = 0,
                        Start = new DateTimeOffset(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, TimeSpan.FromSeconds(0))
                    });
                }

                startDate = startDate.AddDays(1);
            } while (startDate < DateTimeOffset.UtcNow);

            return list.OrderBy(x => x.Start).ToList();
        }
    }
}
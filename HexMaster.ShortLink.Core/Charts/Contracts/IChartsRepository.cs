using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Charts.Models;

namespace HexMaster.ShortLink.Core.Charts.Contracts
{
    public interface IChartsRepository
    {
        Task<List<HourlyHitsDto>> GetHourlyChartAsync(string shortCode, int hours = 24);
        Task<List<DailyHitsDto>> GetDailyChartAsync(string shortCode, int days = 30);
    }
}
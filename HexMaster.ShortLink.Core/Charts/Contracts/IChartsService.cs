using System.Collections.Generic;
using System.Threading.Tasks;
using HexMaster.ShortLink.Core.Charts.Models;

namespace HexMaster.ShortLink.Core.Charts.Contracts
{
    public interface IChartsService
    {
        Task<List<HourlyHitsDto>> GetHourlyChartsAsync(string shortCode);
        Task<List<DailyHitsDto>> GetDailyChartsAsync(string shortCode);
        Task<List<HourlyHitsDto>> GetSparkChartsAsync(string shortCode);
    }
}
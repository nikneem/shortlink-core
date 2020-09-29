using System;

namespace HexMaster.ShortLink.Core.Charts.Models
{
    public sealed class DailyHitsDto
    {
        public DateTimeOffset Start { get; set; }
        public long Hits { get; set; }
    }
}

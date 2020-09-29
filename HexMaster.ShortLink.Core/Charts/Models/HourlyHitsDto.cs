using System;

namespace HexMaster.ShortLink.Core.Charts.Models
{
    public sealed class HourlyHitsDto
    {
        public DateTimeOffset Start { get; set; }
        public string Hour { get; set; }
        public long Hits { get; set; }
    }
}

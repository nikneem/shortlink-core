using System;

namespace HexMaster.ShortLink.Core.Models
{
    public class ShortLinkListItemDto
    {
        public Guid Id { get; set; }
        public string ShortCode { get; set; }
        public string EndpointUrl { get; set; }
        public DateTimeOffset? ExpirationOn { get; set; }
        public int TotalHits { get; set; }
    }
}

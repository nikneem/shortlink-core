using System;

namespace HexMaster.ShortLink.Core.Models.ShortLinks
{
    public class ShortLinkDetailsDto
    {
        public Guid Id { get; set; }
        public string ShortCode { get; set; }
        public string EndpointUrl { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ExpirationOn { get; set; }
        public int TotalHits { get; set; }
    }
}

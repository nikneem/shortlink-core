using System;

namespace HexMaster.ShortLink.Core.Models.ShortLinks
{
    public class ShortLinkUpdateDto
    {
        public Guid Id { get; set; }
        public string EndpointUrl { get; set; }
        public string ShortCode { get; set; }
        public DateTimeOffset? ExpirationOn { get; set; }
    }
}

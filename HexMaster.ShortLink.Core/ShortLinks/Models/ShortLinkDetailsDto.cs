using System;
using HexMaster.ShortLink.Core.Entities;
using HexMaster.ShortLink.Core.ShortLinks.Entities;

namespace HexMaster.ShortLink.Core.ShortLinks.Models
{
    public class ShortLinkDetailsDto
    {
        public Guid Id { get; set; }
        public string ShortCode { get; set; }
        public string EndpointUrl { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ExpirationOn { get; set; }
        public long TotalHits { get; set; }

        internal static ShortLinkDetailsDto CreateFromEntity(ShortLinkEntity entity)
        {
            return new ShortLinkDetailsDto
            {
                Id = Guid.Parse(entity.RowKey),
                EndpointUrl = entity.EndpointUrl,
                ShortCode = entity.ShortCode,
                CreatedOn = entity.CreatedOn,
                ExpirationOn = entity.ExpiresOn,
                TotalHits = entity.TotalHits
            };
        }
    }
}

using System;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Entities
{
    public sealed class ShortLinkEntity : TableEntity
    {
        public string OwnerId { get; set; }
        public string ShortCode { get; set; }
        public string EndpointUrl { get; set; }
        public long TotalHits { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }
    }
}

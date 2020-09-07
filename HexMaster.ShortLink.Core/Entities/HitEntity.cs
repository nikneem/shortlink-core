using System;
using Microsoft.Azure.Cosmos.Table;

namespace HexMaster.ShortLink.Core.Entities
{
    public sealed class HitEntity : TableEntity
    {
        public string ShortCode { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}

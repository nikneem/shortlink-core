using System;

namespace HexMaster.ShortLink.Core.Models.Analytics
{
    public class LinkClickedMessage
    {
        public string Key { get; set; }
        public DateTimeOffset ClickedAt { get; set; }
    }
}
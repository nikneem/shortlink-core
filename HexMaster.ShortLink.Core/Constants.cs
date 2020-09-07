namespace HexMaster.ShortLink.Core
{
    public class Constants
    {
        
    }

    public class HubNames
    {
        public const string ClickEventsHub = "click-events";
    }

    public class TableNames
    {
        public const string Hits = "Hits";
        public const string HitsPerHour = "HitsPerHour";
        public const string HitsPerDay = "HitsPerDay";
        public const string ShortLinks = "ShortLinks";
    }

    public class PartitionKeys
    {
        public const string ShortLinks = "ShortLink";
        public const string Hit = "Hit";
        public const string HitsPerHour = "HitsPerHour";
        public const string HitsPerDay= "HitsPerDay";
    }

}
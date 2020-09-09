namespace HexMaster.ShortLink.Core.Configuration
{
    public class CloudConfiguration
    {
        public const string SectionName = "CloudSettings";

        public string StorageConnectionString { get; set; }
        public string EventHubConnectionString { get; set; }
    }
}
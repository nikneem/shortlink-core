using Microsoft.Extensions.Options;

namespace HexMaster.ShortLink.Core.Configuration
{

    public class CloudConfigurationValidator : IValidateOptions<CloudConfiguration>
    {
        public ValidateOptionsResult Validate(string name, CloudConfiguration options)
        {
            if (options == null)
            {
                return ValidateOptionsResult.Fail($"Object {nameof(options)} may not be null");
            }

            if (string.IsNullOrEmpty(options.EventHubConnectionString))
            {
                return ValidateOptionsResult.Fail(
                    $"Missing configuration setting for {CloudConfiguration.SectionName}:{nameof(options.EventHubConnectionString)}");
            }
            if (string.IsNullOrEmpty(options.StorageConnectionString))
            {
                return ValidateOptionsResult.Fail(
                    $"Missing configuration setting for {CloudConfiguration.SectionName}:{nameof(options.StorageConnectionString)}");
            }

            return ValidateOptionsResult.Success;
        }
    }

}

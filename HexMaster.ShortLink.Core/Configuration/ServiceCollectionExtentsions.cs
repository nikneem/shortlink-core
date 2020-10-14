using HexMaster.ShortLink.Core.Caching;
using HexMaster.ShortLink.Core.Caching.Contracts;
using HexMaster.ShortLink.Core.Charts;
using HexMaster.ShortLink.Core.Charts.Contracts;
using HexMaster.ShortLink.Core.Contracts;
using HexMaster.ShortLink.Core.Helpers;
using HexMaster.ShortLink.Core.Hits;
using HexMaster.ShortLink.Core.Hits.Contracts;
using HexMaster.ShortLink.Core.Repositories;
using HexMaster.ShortLink.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HexMaster.ShortLink.Core.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureCore(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.ConfigureAndValidate<CloudConfiguration, CloudConfigurationValidator>(
                config.GetSection(CloudConfiguration.SectionName));


            serviceCollection.AddScoped<ICloudTableFactory, CloudTableFactory>();
            serviceCollection.AddScoped<IRedisCacheServiceFactory, RedisCacheServiceFactory>();
            serviceCollection.AddScoped<IShortLinksService, ShortLinksService>();
            serviceCollection.AddScoped<IShortLinksRepository, ShortLinksRepository>();
            serviceCollection.AddScoped<IChartsService, ChartsService>();
            serviceCollection.AddScoped<IChartsRepository, ChartsRepository>();
            serviceCollection.AddScoped<IHitsService, HitsService>();
            serviceCollection.AddScoped<IHitsRepository, HitsRepository>();

            serviceCollection.AddSingleton<ShortCodeGenerator>();
        }


        public static IServiceCollection ConfigureAndValidate<T, TValidator>(this IServiceCollection services, IConfiguration configurationSection)
            where T : class, new()
            where TValidator : class, IValidateOptions<T>, new()
        {
            services.Configure<T>(configurationSection);
            services.AddSingleton<IValidateOptions<T>, TValidator>();
            var container = services.BuildServiceProvider();

            try
            {
                var options = container.GetService<IOptions<T>>();
                var validator = new TValidator();
                var validationResult = validator.Validate(string.Empty, options.Value);


                if (validationResult.Failed)
                {
                    var message = $"AppSettings section '{typeof(T).Name}' failed validation. Reason: {validationResult.FailureMessage}";
                    throw new OptionsValidationException(string.Empty, typeof(T), new[] { message });
                }
            }

            finally
            {
                container.Dispose();
            }

            return services;
        }

    }
}

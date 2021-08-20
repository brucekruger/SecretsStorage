using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretsStorage.Models;

namespace SecretsStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecrets(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<Secrets>()
                .Bind(configuration.GetSection(nameof(Secrets)))
                .ValidateDataAnnotations();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISecretsService, SecretsService>();
        }
    }
}
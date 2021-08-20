using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using SecretsStorage.Constants;
using System;

namespace SecretsStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        var keyVaultEndpoint = GetKeyVaultEndpoint();
                        if (!string.IsNullOrEmpty(keyVaultEndpoint))
                        {
                            var azureServiceTokenProvider = new AzureServiceTokenProvider();
                            var keyVaultClientAuthenticationCallback =
                                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);

                            config.AddAzureKeyVault(new AzureKeyVaultConfigurationOptions
                            {
                                Client = new KeyVaultClient(keyVaultClientAuthenticationCallback),
                                Manager = new KeyVaultSecretManager(),
                                ReloadInterval = TimeSpan.FromMinutes(5),
                                Vault = keyVaultEndpoint
                            });
                        }

                        var appConfigConnectionString = GetAppConfigConnectionString();
                        if (!string.IsNullOrEmpty(appConfigConnectionString))
                        {
                            config.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(appConfigConnectionString)
                                .Select(KeyFilter.Any, LabelFilter.Null)
                                .Select(KeyFilter.Any, context.HostingEnvironment.EnvironmentName)
                                .ConfigureRefresh(refresh =>
                                {
                                    refresh
                                        .Register("Version", true)
                                        .SetCacheExpiration(TimeSpan.FromSeconds(30));
                                })
                                .UseFeatureFlags();
                            });
                        }
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static string GetKeyVaultEndpoint()
        {
            var keyVaultName = Environment.GetEnvironmentVariable(EnvironmentVariables.KeyVaultName);
            var keyVaultUrl = string.IsNullOrEmpty(keyVaultName) ? null : $"https://{keyVaultName}.vault.azure.net";
            return keyVaultUrl;
        }

        private static string GetAppConfigConnectionString()
        {
            var connectionString = Environment.GetEnvironmentVariable(EnvironmentVariables.AppConfigConnectionString);
            return connectionString;
        }
    }
}

using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace SecretsStorage
{
    public class KeyVaultSecretManager : DefaultKeyVaultSecretManager
    {
        public override bool Load([NotNull] SecretItem secret) => secret.Attributes.Enabled != null && secret.Attributes.Enabled.Value;
    }
}

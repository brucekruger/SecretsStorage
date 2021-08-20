using Microsoft.Extensions.Options;
using SecretsStorage.Models;

namespace SecretsStorage
{
    internal class SecretsService : ISecretsService
    {
        private readonly Secrets secrets;
        public SecretsService(IOptionsMonitor<Secrets> secrets)
        {
            this.secrets = secrets.CurrentValue;
        }

        public Secrets GetSecrets() => this.secrets;
    }
}
using SecretsStorage.Models;

namespace SecretsStorage
{
    public interface ISecretsService
    {
        Secrets GetSecrets();
    }
}
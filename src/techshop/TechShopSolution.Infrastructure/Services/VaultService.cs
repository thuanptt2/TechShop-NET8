using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using Microsoft.Extensions.Configuration;
using VaultSharp.V1.Commons;
using TechShopSolution.Domain.Services;

namespace TechShopSolution.Infrastructure.Services
{
    public class VaultService : IVaultService
    {
        private readonly IVaultClient _vaultClient;

        public VaultService(IConfiguration configuration)
        {
            string vaultAddress = configuration["Vault:Address"];
            string vaultToken = configuration["Vault:Token"];

            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);
            
            _vaultClient = new VaultClient(new VaultClientSettings(vaultAddress, authMethod));
        }

        // Phương thức lấy secret từ Vault
        public async Task<string> GetSecretAsync(string path, string mountPoint, string key)
        {
            Secret<SecretData> secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, null, mountPoint);

            if (secret.Data.Data.ContainsKey(key))
            {
                return secret.Data.Data[key]?.ToString();
            }

            throw new KeyNotFoundException($"Key '{key}' not found in Vault secret '{path}'.");
        }
    }
}

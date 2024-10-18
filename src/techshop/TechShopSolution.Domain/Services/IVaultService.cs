namespace TechShopSolution.Domain.Services
{
    public interface IVaultService
    {
        Task<string> GetSecretAsync(string path, string mountPoint, string key);
    }
}
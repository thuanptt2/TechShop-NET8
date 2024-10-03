using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Services
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user);
    }
}

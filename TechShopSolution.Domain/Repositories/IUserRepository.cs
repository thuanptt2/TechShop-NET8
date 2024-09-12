using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUserName(string userName);
}

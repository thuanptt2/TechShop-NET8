using Microsoft.EntityFrameworkCore;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Infrastructure.DBContext;

namespace TechShopSolution.Infrastructure.Repositories;

public class UserRepository(TechShopDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByUserName(string userName) 
    {
        return await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }
}

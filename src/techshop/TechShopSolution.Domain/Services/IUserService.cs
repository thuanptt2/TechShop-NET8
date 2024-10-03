using Microsoft.AspNetCore.Identity;
using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Domain.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task AddUserToRoleAsync(User user, string role);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<User> CreateUserAsync(string email, string password);
        Task<IdentityResult> RemoveUserFromRoleAsync(User user, string role);
    }
}

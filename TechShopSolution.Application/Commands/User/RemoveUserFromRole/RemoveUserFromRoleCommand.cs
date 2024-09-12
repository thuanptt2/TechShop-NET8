using MediatR;
using Microsoft.AspNetCore.Identity;
using TechShopSolution.Domain.Entities;

public class RemoveUserFromRoleRequest
{
    public string? UserName { get; set; } 
    public string? RoleName { get; set; }
}

public class RemoveUserFromRoleCommand : IRequest<IdentityResult>
{
    public User? User { get; set; } 
    public string? RoleName { get; set; }
}
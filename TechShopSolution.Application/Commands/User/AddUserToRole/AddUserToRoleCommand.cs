using MediatR;
using TechShopSolution.Domain.Entities;

public class AddUserToRoleRequest
{
    public string? UserName { get; set; } 
    public string? RoleName { get; set; }
}

public class AddUserToRoleCommand : IRequest
{
    public User? User { get; set; } 
    public string? RoleName { get; set; }
}
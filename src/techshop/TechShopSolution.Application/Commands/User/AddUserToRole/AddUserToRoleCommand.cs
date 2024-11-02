using MediatR;
using TechShopSolution.Domain.Models.Common;

public class AddUserToRoleCommand : IRequest<StandardResponse>
{
    public string? UserName { get; set; } 
    public string? RoleName { get; set; }
}
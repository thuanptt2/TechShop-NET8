using MediatR;
using Microsoft.AspNetCore.Identity;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;

public class RemoveUserFromRoleCommand : IRequest<StandardResponse>
{
    public string? UserName { get; set; } 
    public string? RoleName { get; set; }
}
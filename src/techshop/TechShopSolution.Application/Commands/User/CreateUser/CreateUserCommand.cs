using MediatR;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;

public class CreateUserCommand : IRequest<StandardResponse>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
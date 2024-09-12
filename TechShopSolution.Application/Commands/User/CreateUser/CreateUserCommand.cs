using MediatR;
using TechShopSolution.Domain.Entities;

public class CreateUserCommand : IRequest<User>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
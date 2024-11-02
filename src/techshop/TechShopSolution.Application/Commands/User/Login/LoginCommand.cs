using MediatR;
using TechShopSolution.Domain.Models.Common;

public class LoginCommand : IRequest<StandardResponse>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
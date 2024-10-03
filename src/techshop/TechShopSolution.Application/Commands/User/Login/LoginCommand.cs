using MediatR;
using TechShopSolution.Application.Models.Common;

public class LoginCommand : IRequest<StandardResponse<string>>
{
    public string? Email { get; }
    public string? Password { get; }

    public LoginCommand(string? email, string? password)
    {
        Email = email;
        Password = password;
    }
}
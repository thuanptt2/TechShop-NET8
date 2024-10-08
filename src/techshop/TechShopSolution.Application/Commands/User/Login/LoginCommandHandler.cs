using MediatR;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Services;

public class LoginHandler(IUserService userService,
IJwtService jwtService) : IRequestHandler<LoginCommand, StandardResponse<string>>
{
    public async Task<StandardResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
        {
            return new StandardResponse<string>(false) { Message = "Invalid username or password" };
        }

        var token = await jwtService.GenerateJwtToken(user);
        return new StandardResponse<string>(true) { Data = token, Message = "Login successful" };
    }
}
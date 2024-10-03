using MediatR;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Services;

public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand, User>
{

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.CreateUserAsync(request.Email, request.Password);
    }
}
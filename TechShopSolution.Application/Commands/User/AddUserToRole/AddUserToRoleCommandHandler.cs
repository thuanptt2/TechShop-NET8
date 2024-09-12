using MediatR;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Services;

public class AddUserToRoleCommandHandler(IUserService userService) : IRequestHandler<AddUserToRoleCommand>
{

    public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        await userService.AddUserToRoleAsync(request.User, request.RoleName);
    }
}
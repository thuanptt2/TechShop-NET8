using MediatR;
using Microsoft.AspNetCore.Identity;
using TechShopSolution.Domain.Services;

public class RemoveUserFromRoleCommandHandler(IUserService userService) : IRequestHandler<RemoveUserFromRoleCommand, IdentityResult>
{

    public async Task<IdentityResult> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
    {
        return await userService.RemoveUserFromRoleAsync(request.User, request.RoleName);
    }
}
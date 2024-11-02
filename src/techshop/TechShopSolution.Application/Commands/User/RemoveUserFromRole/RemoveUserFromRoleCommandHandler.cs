using MediatR;
using Microsoft.AspNetCore.Identity;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

public class RemoveUserFromRoleCommandHandler(IUserService userService,
IUserRepository userRepository) : IRequestHandler<RemoveUserFromRoleCommand, StandardResponse>
{

    public async Task<StandardResponse> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
    {
        // Tìm user theo username
            var user = await userRepository.GetUserByUserName(request.UserName);
            if (user == null)
            {
                return new StandardResponse
                {
                    Success = false,
                    Message = $"User '{request.UserName}' was not found"
                };
            }

            // Xóa role khỏi user
            var result = await userService.RemoveUserFromRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                return new StandardResponse
                {
                    Success = false,
                    Message = "Failed to remove role",
                    ErrorData = result.Errors
                };
            }

            return new StandardResponse
            {
                Success = true,
                Message = $"Role '{request.RoleName}' removed from user successfully",
                Data = request.RoleName
            };
    }
}
using MediatR;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Domain.Services;

public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, StandardResponse>
{
    private readonly IUserService userService;
    private readonly IUserRepository userRepository;

    public AddUserToRoleCommandHandler(IUserService userService, IUserRepository userRepository)
    {
        this.userService = userService;
        this.userRepository = userRepository;
    }

    public async Task<StandardResponse> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Tìm user theo UserName trong request
            var user = await userRepository.GetUserByUserName(request.UserName);
            if (user == null)
            {
                return new StandardResponse
                {
                    Success = false,
                    Message = $"User '{request.UserName}' was not found",
                };
            }

            // Thêm role cho user
            await userService.AddUserToRoleAsync(user, request.RoleName);

            return new StandardResponse
            {
                Success = true,
                Message = $"Add role '{request.RoleName}' to user '{request.UserName}' successfully",
            };
        }
        catch (Exception ex)
        {
            return new StandardResponse
            {
                Success = false,
                Message = "An error occurred while adding role to user",
                ExceptionMessage = ex.Message
            };
        }
    }
}

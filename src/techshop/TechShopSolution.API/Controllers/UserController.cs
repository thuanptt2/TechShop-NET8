using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechShopSolution.Application.Models.Common;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IMediator mediator, 
IUserRepository userRepository,
ILogger<UserController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try 
        {
            var result = await mediator.Send(new LoginCommand(request.Email, request.Password));
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(JsonConvert.SerializeObject(ex));

            var response = new StandardResponse<string>() {
                Success = false,
                Message = "An unexpected error occurred",
                ExceptionMessage = ex.Message
            };

            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new StandardResponse<object>
            {
                Success = false,
                Message = "Invalid request data",
                ErrorData = ModelState
            });
        }

        try
        {
            var user = await mediator.Send(command, cancellationToken);
            return Ok(new StandardResponse<User>
            {
                Success = true,
                Message = "User created successfully",
                Data = user
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponse<object>
            {
                Success = false,
                Message = "An error occurred while creating the user",
                ExceptionMessage = ex.Message
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(AddUserToRoleRequest request)
    {
        try 
        {
            var user = await userRepository.GetUserByUserName(request.UserName);
        
            if(user == null)
            {
                return NotFound(new StandardResponse<string>
                {
                    Success = false,
                    Message = $"User '{request.UserName}' was not found",
                });
            }

            await mediator.Send(new AddUserToRoleCommand {
                User = user,
                RoleName = request.RoleName
            } );

            return Ok(new StandardResponse<string>
            {
                Success = true,
                Message = $"Add role '{request.RoleName}' to user '{request.UserName}' succesfully",
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new StandardResponse<object>
            {
                Success = false,
                Message = "An error occurred while adding role to user",
                ExceptionMessage = ex.Message
            });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RemoveUserFromRole")]
    public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleRequest request)
    {
        var user = await userRepository.GetUserByUserName(request.UserName);
        if (user == null)
        {
            return NotFound(new StandardResponse<object>
            {
                Success = false,
                Message = $"User '{request.UserName}' was not found",
            });
        }

        var result = await mediator.Send(new RemoveUserFromRoleCommand {
            User = user,
            RoleName = request.RoleName
        } );

        if (result.Succeeded)
        {
            return Ok(new StandardResponse<string>
            {
                Success = true,
                Message = $"Role '{request.RoleName}' removed from user successfully",
                Data = request.RoleName
            });
        }
        else
        {
            return BadRequest(new StandardResponse<object>
            {
                Success = false,
                Message = "Failed to remove role",
                ErrorData = result.Errors
            });
        }
    }

}

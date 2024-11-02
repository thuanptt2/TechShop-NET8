using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechShopSolution.Domain.Models.Common;
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
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var response = await mediator.Send(command);

        if (!response.Success) 
        {
            BadRequest(response);
        }

        return Ok(response);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command);

        if (!response.Success) 
        {
            BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddUserToRole")]
    public async Task<IActionResult> AddUserToRole(AddUserToRoleCommand command)
    {
        var response = await mediator.Send(command);

        if (!response.Success) 
        {
            return response.Data == null ? NotFound(response) : BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RemoveUserFromRole")]
    public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleCommand command)
    {
        var response = await mediator.Send(command);

        if (!response.Success) 
        {
            return response.Data == null ? NotFound(response) : BadRequest(response);
        }

        return Ok(response);
    }

}

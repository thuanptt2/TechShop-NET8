using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Common;
using TechShopSolution.Domain.Services;

public class CreateUserCommandHandler(IUserService userService, 
ILogger<CreateUserCommandHandler> logger,
CreateUserCommandValidator validator) : IRequestHandler<CreateUserCommand, StandardResponse>
{

    public async Task<StandardResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

            return new StandardResponse
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            };
        }

        // Tạo user
        var user = await userService.CreateUserAsync(request.Email, request.Password);
        return new StandardResponse
        {
            Success = true,
            Message = "User created successfully",
            Data = user
        };
    }
}
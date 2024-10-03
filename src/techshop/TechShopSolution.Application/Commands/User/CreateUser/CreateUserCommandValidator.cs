using FluentValidation;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{

    public CreateUserCommandValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email invalid");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters");
    }
}
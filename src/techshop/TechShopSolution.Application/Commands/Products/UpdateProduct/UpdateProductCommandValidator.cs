using FluentValidation;

namespace TechShopSolution.Application.Commands.Products.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{

    public UpdateProductCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Product name is required");

        RuleFor(dto => dto.Code)
            .NotEmpty().WithMessage("Product code is required")
            .Length(3, 20).WithMessage("Product code must be between 3 and 20 characters");

        RuleFor(dto => dto.Slug)
            .Matches(@"^\S*$")
            .WithMessage("Slug cannot contain whitespace");
    }
}
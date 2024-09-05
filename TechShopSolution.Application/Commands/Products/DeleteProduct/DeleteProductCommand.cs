using MediatR;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Commands.Products.DeleteProduct;

public class DeleteProductCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}
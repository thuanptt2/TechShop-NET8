using MediatR;

namespace TechShopSolution.Application.Commands.Products.DeleteProduct;

public class DeleteProductCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}
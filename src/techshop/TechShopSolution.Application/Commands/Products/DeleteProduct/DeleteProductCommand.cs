using MediatR;
using TechShopSolution.Domain.Models.Common;

namespace TechShopSolution.Application.Commands.Products.DeleteProduct;

public class DeleteProductCommand(int id) : IRequest<StandardResponse>
{
    public int Id { get; set; } = id;
}
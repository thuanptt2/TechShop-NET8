using AutoMapper;
using MediatR;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Commands.Products.CreateProduct;

public class CreateProductCommandHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);

        int id = await productRepository.Create(product);
        return id;
    }
}
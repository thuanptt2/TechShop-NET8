using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Repositories;

namespace TechShopSolution.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductV2QueryHandler : IRequestHandler<GetAllProductV2Query, (IEnumerable<ProductDTO>, int)>
    {
        private readonly ILogger<GetAllProductV2QueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetAllProductV2QueryHandler(ILogger<GetAllProductV2QueryHandler> logger, 
            IMapper mapper, 
            IProductRepository productRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<(IEnumerable<ProductDTO>, int)> Handle(GetAllProductV2Query request, CancellationToken cancellationToken)
        {
            var totalRecords = await _productRepository.CountAsync();

            var products = await _productRepository.GetPagedAsync(request.PageNumber, request.PageSize);

            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return (productDTOs, totalRecords);
        }
    }
}

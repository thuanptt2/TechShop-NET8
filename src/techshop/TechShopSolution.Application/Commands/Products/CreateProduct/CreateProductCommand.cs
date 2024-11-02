using MediatR;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Common;

namespace TechShopSolution.Application.Commands.Products.CreateProduct;

public class CreateProductCommand : IRequest<StandardResponse>
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Slug { get; set; }
    public string? Image { get; set; }
    public string? MoreImages { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PromotionPrice { get; set; }
    public int Warranty { get; set; }
    public int? Instock { get; set; }
    public string? Specifications { get; set; }
    public string? ShortDesc { get; set; }
    public string? Descriptions { get; set; }
    public bool Featured { get; set; }
    public bool BestSeller { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeywords { get; set; }
    public string? MetaDescriptions { get; set; }
    public int BrandId { get; set; }
    public List<CategoryProductDTO>? ProductInCategory { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
}
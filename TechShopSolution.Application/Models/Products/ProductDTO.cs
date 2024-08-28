using TechShopSolution.Application.Models.Brands;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Models.Products
{
    public class ProductDTO 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Slug { get; set; }
        public int BrandId { get; set; }
        public string? Image { get; set; }
        public string? MoreImages { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int Warranty { get; set; }
        public int? Instock { get; set; }
        public int ViewCount { get; set; }
        public BrandDTO? Brand { get; set; }
    }       
}
using System.ComponentModel.DataAnnotations;
using TechShopSolution.Application.Models.Categories;

namespace TechShopSolution.Application.Models.Products
{
    public class CreateProductDTO 
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = default!;
        
        [Required(ErrorMessage = "Product code is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Product code must be between 3 and 20 characters")]
        public string Code { get; set; } = default!;

        [RegularExpression(@"^\S*$", ErrorMessage = "Slug cannot contain whitespace")]
        public string? Slug { get; set; }
        public string? Image { get; set; }
        public string? MoreImages { get; set; }
        [Required(ErrorMessage = "Product price is required")]
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
    }       
}
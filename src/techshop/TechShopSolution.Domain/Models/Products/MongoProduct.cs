namespace TechShopSolution.Domain.Models;
public class MongoProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Slug { get; set; }
    public int BrandId { get; set; }
    public List<int>? Categories {get; set; }
    public string? Image { get; set; }
    public string? MoreImages { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal PromotionPrice { get; set; }
    public int Warranty { get; set; }
    public int? Instock { get; set; }
    public int ViewCount { get; set; }
    public string? Specifications { get; set; }
    public string? ShortDesc { get; set; }
    public string? Descriptions { get; set; }
    public bool Featured { get; set; }
    public bool BestSeller { get; set; }
    public bool IsActive { get; set; }
    public bool IsDelete { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaKeywords { get; set; }
    public string? MetaDescriptions { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeleteAt { get; set; }
}

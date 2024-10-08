namespace TechShopSolution.Domain.Models.Brands
{
    public class BrandDTO 
    {
        public int Id { get; set; }
        public string? BrandName { get; set; }
        public string? BrandSlug { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }       
}
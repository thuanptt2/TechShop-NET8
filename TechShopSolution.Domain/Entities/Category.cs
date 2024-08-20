using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? CateName { get; set; }
        public string? CateSlug { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescriptions { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public List<CategoryProduct>? ProductInCategory { get; set; }
    }
}

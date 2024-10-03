using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class CategoryProduct
    {
        public int ProductId { get; set; }    
        public int CateId { get; set; }
        public Product? Product { get; set; }
        public Category? Category { get; set; }
    }
}

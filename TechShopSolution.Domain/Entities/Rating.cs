using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Rating
    {
        public int ProductId { get; set; }
        public int CusId { get; set; }
        public int Score { get; set; }
        public string? Content { get; set; }
        public DateTime DateRating { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
    }
}

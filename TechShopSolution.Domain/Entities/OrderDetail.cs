using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? PromotionPrice { get; set; }
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}

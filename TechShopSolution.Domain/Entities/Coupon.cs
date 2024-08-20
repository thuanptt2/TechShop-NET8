using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double? MinOrderValue { get; set; }
        public double? MaxPrice { get; set; }
        public double Value { get; set; }
        public int? Quantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public List<Order>? Orders { get; set; }
    }
}

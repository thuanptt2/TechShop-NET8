using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Transporter
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public List<Transport>? Transports { get; set; }
    }
}

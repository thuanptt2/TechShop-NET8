using System;
using System.Collections.Generic;
using System.Text;

namespace TechShopSolution.Domain.Entities
{
    public class Favorite
    {
        public int ProductId { get; set; }
        public int CusId { get; set; }
        public DateTime DateFavorite { get; set; }
        public Product? Product { get; set; }
        public Customer? Customer { get; set; }
    }
}

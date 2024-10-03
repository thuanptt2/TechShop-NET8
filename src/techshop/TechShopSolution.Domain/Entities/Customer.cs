namespace TechShopSolution.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public DateTime Birthday { get; set; }
        public bool Sex { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public List<Order>? Order { get; set; }
        public List<Rating>? Ratings { get; set; }
        public List<Favorite>? FavoriteProducts { get; set; }
    }
}

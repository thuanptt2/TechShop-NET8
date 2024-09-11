using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechShopSolution.Domain.Entities;

namespace TechShopSolution.Infrastructure.DBContext
{
    public class TechShopDbContext : IdentityDbContext<User>
    {
        public TechShopDbContext(DbContextOptions<TechShopDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrDetails { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
          base.OnModelCreating(builder);

            builder.Entity<Admin>().ToTable("Admin");
            builder.Entity<Brand>().ToTable("Brand");
            builder.Entity<Category>().ToTable("Category");
            builder.Entity<CategoryProduct>().ToTable("CategoryProduct");
            builder.Entity<Coupon>().ToTable("Coupon");
            builder.Entity<Customer>().ToTable("Customer");
            builder.Entity<Favorite>().ToTable("Favorite");
            builder.Entity<Order>().ToTable("Order");
            builder.Entity<OrderDetail>().ToTable("OrderDetail");
            builder.Entity<PaymentMethod>().ToTable("PaymentMethod");
            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Rating>().ToTable("Rating");
            builder.Entity<Transport>().ToTable("Transport");
            builder.Entity<Transporter>().ToTable("Transporter");

            // Column Configuration
            builder.Entity<Order>().Property(p => p.Total).HasColumnType("MONEY");
            builder.Entity<Order>().Property(p => p.Discount).HasColumnType("MONEY");
            builder.Entity<Order>().Property(p => p.TransportFee).HasColumnType("MONEY");

            builder.Entity<OrderDetail>().Property(p => p.UnitPrice).HasColumnType("MONEY");
            builder.Entity<OrderDetail>().Property(p => p.PromotionPrice).HasColumnType("MONEY");

            builder.Entity<Product>().Property(p => p.PromotionPrice).HasColumnType("MONEY");
            builder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("MONEY");

             builder.Entity<Transport>().Property(p => p.CodPrice).HasColumnType("MONEY");

            // Relationship configuration
            builder.Entity<Product>()
                .HasOne(x => x.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(x => x.BrandId);

            builder.Entity<CategoryProduct>()
                .HasKey(t => new { t.CateId, t.ProductId });
            builder.Entity<CategoryProduct>()
                .HasOne(t => t.Product)
                .WithMany(c => c.ProductInCategory)
                .HasForeignKey(c => c.ProductId);
            builder.Entity<CategoryProduct>()
                .HasOne(t => t.Category)
                .WithMany(c => c.ProductInCategory)
                .HasForeignKey(c => c.CateId);

            builder.Entity<Favorite>()
                .HasKey(x => new { x.ProductId, x.CusId });
            builder.Entity<Favorite>()
                .HasOne(x => x.Product)
                .WithMany(t => t.Favoriters)
                .HasForeignKey(x => x.ProductId);
            builder.Entity<Favorite>()
                .HasOne(x => x.Customer)
                .WithMany(t => t.FavoriteProducts)
                .HasForeignKey(x => x.CusId);

            builder.Entity<Order>()
                .HasOne(x => x.Customers)
                .WithMany(t => t.Order)
                .HasForeignKey(x => x.CusId);
            builder.Entity<Order>()
                .HasOne(x => x.Coupon)
                .WithMany(t => t.Orders)
                .HasForeignKey(x => x.CouponId);
            builder.Entity<Order>()
                .HasOne(x => x.PaymentMethod)
                .WithMany(t => t.Orders)
                .HasForeignKey(x => x.PaymentId);
            builder.Entity<Order>()
                .HasOne(x => x.Transport)
                .WithOne(t => t.Order)
                .HasForeignKey<Transport>(t => t.OrderId);

            builder.Entity<OrderDetail>()
                .HasKey(x => new { x.OrderId, x.ProductId });
            builder.Entity<OrderDetail>()
                .HasOne(x => x.Order)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(x => x.OrderId);
            builder.Entity<OrderDetail>()
                .HasOne(x => x.Product)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(x => x.ProductId);

            builder.Entity<Rating>()
                .HasKey(x => new { x.ProductId, x.CusId });
            builder.Entity<Rating>()
                .HasOne(x => x.Product)
                .WithMany(t => t.Ratings)
                .HasForeignKey(x => x.ProductId);
            builder.Entity<Rating>()
                .HasOne(x => x.Customer)
                .WithMany(t => t.Ratings)
                .HasForeignKey(x => x.CusId);

            builder.Entity<Transport>()
                .HasOne(x => x.Transporter)
                .WithMany(t => t.Transports)
                .HasForeignKey(x => x.TransporterId);
        }
    }
}
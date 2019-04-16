using Microsoft.EntityFrameworkCore;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{
    public class TrojantradingDbContext : DbContext
    {
        public TrojantradingDbContext(DbContextOptions options) : base(options)
        {
        }

        public TrojantradingDbContext() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<HeadInformation> HeadInformations { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<PdfBoard> PdfBoards { get; set; }
        public DbSet<CompanyInfo> CompanyInfos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<BillingAddress> BillingAddress { get; set; }
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("trojantrading");
            modelBuilder.Entity<User>()
                .ToTable("user")
                .HasKey(u => u.Id);
            modelBuilder.Entity<Product>()
                .ToTable("product")
                .HasKey(p => p.Id);
            modelBuilder.Entity<Order>()
                .ToTable("order")
                .HasKey(o => o.Id);
            modelBuilder.Entity<ShoppingCart>()
                .ToTable("shoppingCart")
                .HasKey(s => s.Id);
            modelBuilder.Entity<ShoppingItem>()
                .ToTable("shoppingItem")
                .HasKey(s => s.Id);
            modelBuilder.Entity<PdfBoard>()
                .ToTable("pdfBoard")
                .HasKey(p => p.Id);
            modelBuilder.Entity<HeadInformation>()
                .ToTable("headInformation")
                .HasKey(h => h.Id);
            modelBuilder.Entity<CompanyInfo>()
                .ToTable("companyInfo")
                .HasKey(c => c.Id);
            modelBuilder.Entity<Role>()
                .ToTable("role")
                .HasKey(r => r.Id);
            modelBuilder.Entity<ShippingAddress>()
                .ToTable("ShippingAddress")
                .HasKey(s => s.Id);
            modelBuilder.Entity<BillingAddress>()
                .ToTable("BillingAddress")
                .HasKey(s => s.Id);

            //user shoppingcart 1:1
            modelBuilder.Entity<User>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(s => s.User)
                .HasForeignKey<ShoppingCart>(s => s.UserId);

            //user order 1:m
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            //role user 1:m
            modelBuilder.Entity<User>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId);

            //order shoppingitem 1:m
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.Order)
                .WithMany(o => o.ShoppingItems)
                .HasForeignKey(s => s.OrderId);

            //shopping Cart Shopping Item 1:m
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.ShoppingCart)
                .WithMany(s => s.ShoppingItems)
                .HasForeignKey(s => s.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);

            //shopping item product 1:1
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.Product)
                .WithOne(p => p.ShoppingItem)
                .HasForeignKey<ShoppingItem>(p => p.ProductId);

            // user Ship address 1:1
            modelBuilder.Entity<User>()
                .HasOne(u => u.ShippingAddress)
                .WithOne(s => s.User)
                .HasForeignKey<ShippingAddress>(s => s.UserId);
            // user Bill address 1:1
            modelBuilder.Entity<User>()
                .HasOne(u => u.BillingAddress)
                .WithOne(s => s.User)
                .HasForeignKey<BillingAddress>(s => s.UserId);
        }
    }
}
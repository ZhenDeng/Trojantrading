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

            //user shoppingcart 1:m
            modelBuilder.Entity<User>()
                .HasMany(u => u.ShoppingCarts)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId);

            //user order 1:m
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            //order shoppingcart 1:1
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.Order)
                .WithOne(o => o.ShoppingCart)
                .HasForeignKey<Order>(o => o.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);

            //shopping Cart Shopping Item 1:m
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.ShoppingCart)
                .WithMany(s => s.ShoppingItems)
                .HasForeignKey(s => s.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);

            //shopping item product 1:1
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.Product)
                .WithMany(p => p.ShoppingItems)
                .HasForeignKey(s => s.ProductId);
        }
    }
}
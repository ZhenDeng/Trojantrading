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
            //modelBuilder.Entity<ShoppingItem>()
            //    .ToTable("shoppingItem")
            //    .HasKey(s => s.Id);
            modelBuilder.Entity<UserRole>()
                .ToTable("userrole")
                .HasKey(ur=>new{ur.UserId, ur.RoleId});


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

            //user role m:n
            modelBuilder.Entity<UserRole>()
                .HasOne(ur=>ur.User)
                .WithOne(u=>u.UserRole)
                .HasForeignKey<UserRole>(ur=>ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur=>ur.Role)
                .WithMany(r=>r.UserRoles)
                .HasForeignKey(ur=>ur.RoleId);

            //order shoppingitem 1:m
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s=>s.Order)
                .WithMany(o=>o.ShoppingItems)
                .HasForeignKey(s=>s.OrderId);

            //shopping Cart Shopping Item 1:m
            modelBuilder.Entity<ShoppingItem>()
                .HasOne(s => s.ShoppingCart)
                .WithMany(s => s.ShoppingItems)
                .HasForeignKey(s => s.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);

            //shopping item product 1:1
            modelBuilder.Entity<Product>()
                .HasOne(p=>p.ShoppingItem)
                .WithOne(s=>s.Product)
                .HasForeignKey<Product>(p=>p.ShoppingItemId);

            //seed data into the databases
  
            //seed data
            // modelBuilder.Entity<CompanyInfo>()
            //     .HasData(
            //         new {}
            //     );
        }
    }
}
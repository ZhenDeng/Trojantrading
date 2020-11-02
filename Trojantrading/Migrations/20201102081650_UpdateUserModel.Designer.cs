﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Trojantrading.Repositories;

namespace Trojantrading.Migrations
{
    [DbContext(typeof(TrojantradingDbContext))]
    [Migration("20201102081650_UpdateUserModel")]
    partial class UpdateUserModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("trojantrading")
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Trojantrading.Models.HeadInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("ImagePath");

                    b.HasKey("Id");

                    b.ToTable("headInformation");
                });

            modelBuilder.Entity("Trojantrading.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminMessage");

                    b.Property<double>("Balance");

                    b.Property<string>("ClientMessage");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("InvoiceNo");

                    b.Property<string>("OrderStatus");

                    b.Property<int>("ShoppingCartId");

                    b.Property<int>("TotalItems");

                    b.Property<double>("TotalPrice");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCartId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("order");
                });

            modelBuilder.Entity("Trojantrading.Models.PackagingList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PackageName");

                    b.Property<int>("ProductId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("packagingList");
                });

            modelBuilder.Entity("Trojantrading.Models.PdfBoard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Path");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("pdfBoard");
                });

            modelBuilder.Entity("Trojantrading.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AgentPrice");

                    b.Property<string>("Category");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("ItemCode");

                    b.Property<int?>("MaxQty");

                    b.Property<int?>("MinQty");

                    b.Property<string>("Name");

                    b.Property<double>("OriginalPrice");

                    b.Property<double>("PrepaymentDiscount");

                    b.Property<string>("Status");

                    b.Property<double>("WholesalerPrice");

                    b.HasKey("Id");

                    b.ToTable("product");
                });

            modelBuilder.Entity("Trojantrading.Models.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Note");

                    b.Property<double>("OriginalPrice");

                    b.Property<string>("PaymentMethod");

                    b.Property<string>("Status");

                    b.Property<int>("TotalItems");

                    b.Property<double>("TotalPrice");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("shoppingCart");
                });

            modelBuilder.Entity("Trojantrading.Models.ShoppingItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<string>("Packaging");

                    b.Property<int>("ProductId");

                    b.Property<int>("ShoppingCartId");

                    b.Property<string>("Status");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartId");

                    b.HasIndex("UserId");

                    b.ToTable("shoppingItem");
                });

            modelBuilder.Entity("Trojantrading.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abn");

                    b.Property<string>("Account");

                    b.Property<string>("Acn");

                    b.Property<string>("BillingAddressLine");

                    b.Property<string>("BillingCustomerName");

                    b.Property<string>("BillingPostCode");

                    b.Property<string>("BillingState");

                    b.Property<string>("BillingStreetNumber");

                    b.Property<string>("BillingSuburb");

                    b.Property<string>("BussinessName");

                    b.Property<string>("CompanyAddress");

                    b.Property<string>("CompanyEmail");

                    b.Property<string>("CompanyPhone");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email");

                    b.Property<string>("Fax");

                    b.Property<string>("Mobile");

                    b.Property<string>("PassswordHash");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<string>("Role");

                    b.Property<bool>("SendEmail");

                    b.Property<string>("ShippingAddressLine");

                    b.Property<string>("ShippingCustomerName");

                    b.Property<string>("ShippingPostCode");

                    b.Property<string>("ShippingState");

                    b.Property<string>("ShippingStreetNumber");

                    b.Property<string>("ShippingSuburb");

                    b.Property<string>("Status");

                    b.Property<string>("Trn");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Trojantrading.Models.Order", b =>
                {
                    b.HasOne("Trojantrading.Models.ShoppingCart", "ShoppingCart")
                        .WithOne("Order")
                        .HasForeignKey("Trojantrading.Models.Order", "ShoppingCartId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Trojantrading.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Trojantrading.Models.PackagingList", b =>
                {
                    b.HasOne("Trojantrading.Models.Product")
                        .WithMany("PackagingLists")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Trojantrading.Models.ShoppingCart", b =>
                {
                    b.HasOne("Trojantrading.Models.User", "User")
                        .WithMany("ShoppingCarts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Trojantrading.Models.ShoppingItem", b =>
                {
                    b.HasOne("Trojantrading.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Trojantrading.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingItems")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Trojantrading.Models.User")
                        .WithMany("ShoppingItems")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trojantrading.Migrations
{
    public partial class UpdateUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.EnsureSchema(
            //    name: "trojantrading");

            //migrationBuilder.CreateTable(
            //    name: "headInformation",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Content = table.Column<string>(nullable: true),
            //        ImagePath = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_headInformation", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "pdfBoard",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Title = table.Column<string>(nullable: true),
            //        Path = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_pdfBoard", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "product",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        Name = table.Column<string>(nullable: true),
            //        ItemCode = table.Column<string>(nullable: true),
            //        OriginalPrice = table.Column<double>(nullable: false),
            //        MaxQty = table.Column<int>(nullable: true),
            //        MinQty = table.Column<int>(nullable: true),
            //        AgentPrice = table.Column<double>(nullable: false),
            //        WholesalerPrice = table.Column<double>(nullable: false),
            //        PrepaymentDiscount = table.Column<double>(nullable: false),
            //        Category = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_product", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "user",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        Account = table.Column<string>(nullable: true),
            //        PassswordHash = table.Column<string>(nullable: true),
            //        Password = table.Column<string>(nullable: true),
            //        BussinessName = table.Column<string>(nullable: true),
            //        Trn = table.Column<string>(nullable: true),
            //        Email = table.Column<string>(nullable: true),
            //        Mobile = table.Column<string>(nullable: true),
            //        Phone = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true),
            //        SendEmail = table.Column<bool>(nullable: false),
            //        BillingCustomerName = table.Column<string>(nullable: true),
            //        BillingStreetNumber = table.Column<string>(nullable: true),
            //        BillingAddressLine = table.Column<string>(nullable: true),
            //        BillingSuburb = table.Column<string>(nullable: true),
            //        BillingState = table.Column<string>(nullable: true),
            //        BillingPostCode = table.Column<string>(nullable: true),
            //        ShippingCustomerName = table.Column<string>(nullable: true),
            //        ShippingStreetNumber = table.Column<string>(nullable: true),
            //        ShippingAddressLine = table.Column<string>(nullable: true),
            //        ShippingSuburb = table.Column<string>(nullable: true),
            //        ShippingState = table.Column<string>(nullable: true),
            //        ShippingPostCode = table.Column<string>(nullable: true),
            //        CompanyAddress = table.Column<string>(nullable: true),
            //        CompanyEmail = table.Column<string>(nullable: true),
            //        CompanyPhone = table.Column<string>(nullable: true),
            //        Fax = table.Column<string>(nullable: true),
            //        Abn = table.Column<string>(nullable: true),
            //        Acn = table.Column<string>(nullable: true),
            //        Role = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_user", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "packagingList",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        PackageName = table.Column<string>(nullable: true),
            //        ProductId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_packagingList", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_packagingList_product_ProductId",
            //            column: x => x.ProductId,
            //            principalSchema: "trojantrading",
            //            principalTable: "product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "shoppingCart",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        TotalItems = table.Column<int>(nullable: false),
            //        TotalPrice = table.Column<double>(nullable: false),
            //        OriginalPrice = table.Column<double>(nullable: false),
            //        UserId = table.Column<int>(nullable: false),
            //        Note = table.Column<string>(nullable: true),
            //        PaymentMethod = table.Column<string>(nullable: true),
            //        Status = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_shoppingCart", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_shoppingCart_user_UserId",
            //            column: x => x.UserId,
            //            principalSchema: "trojantrading",
            //            principalTable: "user",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "order",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        CreatedDate = table.Column<DateTime>(nullable: false),
            //        TotalItems = table.Column<int>(nullable: false),
            //        TotalPrice = table.Column<double>(nullable: false),
            //        Balance = table.Column<double>(nullable: false),
            //        OrderStatus = table.Column<string>(nullable: true),
            //        ClientMessage = table.Column<string>(nullable: true),
            //        AdminMessage = table.Column<string>(nullable: true),
            //        UserId = table.Column<int>(nullable: false),
            //        ShoppingCartId = table.Column<int>(nullable: false),
            //        InvoiceNo = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_order", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_order_shoppingCart_ShoppingCartId",
            //            column: x => x.ShoppingCartId,
            //            principalSchema: "trojantrading",
            //            principalTable: "shoppingCart",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_order_user_UserId",
            //            column: x => x.UserId,
            //            principalSchema: "trojantrading",
            //            principalTable: "user",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "shoppingItem",
            //    schema: "trojantrading",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Amount = table.Column<int>(nullable: false),
            //        ProductId = table.Column<int>(nullable: false),
            //        Packaging = table.Column<string>(nullable: true),
            //        ShoppingCartId = table.Column<int>(nullable: false),
            //        Status = table.Column<string>(nullable: true),
            //        UserId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_shoppingItem", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_shoppingItem_product_ProductId",
            //            column: x => x.ProductId,
            //            principalSchema: "trojantrading",
            //            principalTable: "product",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_shoppingItem_shoppingCart_ShoppingCartId",
            //            column: x => x.ShoppingCartId,
            //            principalSchema: "trojantrading",
            //            principalTable: "shoppingCart",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_shoppingItem_user_UserId",
            //            column: x => x.UserId,
            //            principalSchema: "trojantrading",
            //            principalTable: "user",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_order_ShoppingCartId",
            //    schema: "trojantrading",
            //    table: "order",
            //    column: "ShoppingCartId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_order_UserId",
            //    schema: "trojantrading",
            //    table: "order",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_packagingList_ProductId",
            //    schema: "trojantrading",
            //    table: "packagingList",
            //    column: "ProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_shoppingCart_UserId",
            //    schema: "trojantrading",
            //    table: "shoppingCart",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_shoppingItem_ProductId",
            //    schema: "trojantrading",
            //    table: "shoppingItem",
            //    column: "ProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_shoppingItem_ShoppingCartId",
            //    schema: "trojantrading",
            //    table: "shoppingItem",
            //    column: "ShoppingCartId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_shoppingItem_UserId",
            //    schema: "trojantrading",
            //    table: "shoppingItem",
            //    column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "headInformation",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "order",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "packagingList",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "pdfBoard",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "shoppingItem",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "product",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "shoppingCart",
            //    schema: "trojantrading");

            //migrationBuilder.DropTable(
            //    name: "user",
            //    schema: "trojantrading");
        }
    }
}

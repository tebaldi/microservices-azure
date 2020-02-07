using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchT.Services.Search.Infrastructure.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SearchDb");

            migrationBuilder.CreateTable(
                name: "OrdersReport",
                schema: "SearchDb",
                columns: table => new
                {
                    OrderId = table.Column<string>(nullable: false),
                    OrderStatus = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderLines = table.Column<int>(nullable: false),
                    OrderTotal = table.Column<decimal>(nullable: false),
                    ProductId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersReport", x => new { x.OrderId, x.ProductId });
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "SearchDb",
                columns: table => new
                {
                    EventName = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    EventSequence = table.Column<long>(nullable: false),
                    ProductId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Stock = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersReport",
                schema: "SearchDb");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "SearchDb");
        }
    }
}

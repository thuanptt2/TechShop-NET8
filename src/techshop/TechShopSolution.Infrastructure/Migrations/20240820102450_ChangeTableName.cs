using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechShopSolution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetaTittle",
                table: "Product",
                newName: "MetaTitle");

            migrationBuilder.AlterColumn<decimal>(
                name: "CodPrice",
                table: "Transport",
                type: "MONEY",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MetaTitle",
                table: "Product",
                newName: "MetaTittle");

            migrationBuilder.AlterColumn<decimal>(
                name: "CodPrice",
                table: "Transport",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "MONEY");
        }
    }
}

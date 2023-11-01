using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialOrderDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailMessage",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailMessage",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductId",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

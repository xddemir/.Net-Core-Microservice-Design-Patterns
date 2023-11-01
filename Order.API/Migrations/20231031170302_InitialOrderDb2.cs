using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialOrderDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Type_Orders_OrderId",
                table: "Type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Type",
                table: "Type");

            migrationBuilder.RenameTable(
                name: "Type",
                newName: "OrderItems");

            migrationBuilder.RenameIndex(
                name: "IX_Type_OrderId",
                table: "OrderItems",
                newName: "IX_OrderItems_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "Type");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_OrderId",
                table: "Type",
                newName: "IX_Type_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Type",
                table: "Type",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Type_Orders_OrderId",
                table: "Type",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

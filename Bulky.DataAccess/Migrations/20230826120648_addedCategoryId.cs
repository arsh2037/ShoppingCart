using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Product",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                newName: "IX_Product_categoryId");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                column: "categoryId",
                value: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Categories_categoryId",
                table: "Product",
                column: "categoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Categories_categoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Product",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_categoryId",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Categories_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

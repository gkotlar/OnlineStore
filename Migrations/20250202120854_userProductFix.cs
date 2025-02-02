using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class userProductFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_Product_ProductId",
                table: "UserProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "UserProduct",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SellerProductId",
                table: "UserProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProduct_SellerProductId",
                table: "UserProduct",
                column: "SellerProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_Product_ProductId",
                table: "UserProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_SellerProduct_SellerProductId",
                table: "UserProduct",
                column: "SellerProductId",
                principalTable: "SellerProduct",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_Product_ProductId",
                table: "UserProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProduct_SellerProduct_SellerProductId",
                table: "UserProduct");

            migrationBuilder.DropIndex(
                name: "IX_UserProduct_SellerProductId",
                table: "UserProduct");

            migrationBuilder.DropColumn(
                name: "SellerProductId",
                table: "UserProduct");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "UserProduct",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProduct_Product_ProductId",
                table: "UserProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class AddedWholesaleAndReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnDisplay",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnPage",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WholesalerId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wholesalers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wholesalers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_WholesalerId",
                table: "Products",
                column: "WholesalerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Wholesalers_WholesalerId",
                table: "Products",
                column: "WholesalerId",
                principalTable: "Wholesalers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Wholesalers_WholesalerId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Wholesalers");

            migrationBuilder.DropIndex(
                name: "IX_Products_WholesalerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsOnDisplay",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsOnPage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WholesalerId",
                table: "Products");
        }
    }
}

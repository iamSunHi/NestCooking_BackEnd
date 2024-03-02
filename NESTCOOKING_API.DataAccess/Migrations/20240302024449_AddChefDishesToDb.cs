using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddChefDishesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChefDishes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Portion = table.Column<int>(type: "int", nullable: false),
                    RecipeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChefDishes_Users_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChefDishes_ChefId",
                table: "ChefDishes",
                column: "ChefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChefDishes");
        }
    }
}

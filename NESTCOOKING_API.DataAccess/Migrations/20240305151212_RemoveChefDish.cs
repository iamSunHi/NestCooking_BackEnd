using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChefDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedRecipes_Transactions_TransactionId",
                table: "PurchasedRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedRecipes_Users_UserId",
                table: "PurchasedRecipes");

            migrationBuilder.DropTable(
                name: "ChefDishes");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedRecipes_Transactions_TransactionId",
                table: "PurchasedRecipes",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedRecipes_Users_UserId",
                table: "PurchasedRecipes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedRecipes_Transactions_TransactionId",
                table: "PurchasedRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedRecipes_Users_UserId",
                table: "PurchasedRecipes");

            migrationBuilder.CreateTable(
                name: "ChefDishes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Portion = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RecipeUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedRecipes_Transactions_TransactionId",
                table: "PurchasedRecipes",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedRecipes_Users_UserId",
                table: "PurchasedRecipes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

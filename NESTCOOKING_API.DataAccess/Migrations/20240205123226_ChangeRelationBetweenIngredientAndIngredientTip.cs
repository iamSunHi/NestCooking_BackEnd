using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationBetweenIngredientAndIngredientTip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientTips_Ingredients_IngredientId",
                table: "IngredientTips");

            migrationBuilder.DropIndex(
                name: "IX_IngredientTips_IngredientId",
                table: "IngredientTips");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "IngredientTips");

            migrationBuilder.AddColumn<int>(
                name: "IngredientTipId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientTipId",
                table: "Ingredients",
                column: "IngredientTipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientTips_IngredientTipId",
                table: "Ingredients",
                column: "IngredientTipId",
                principalTable: "IngredientTips",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientTips_IngredientTipsId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientTipsId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "IngredientTipsId",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "IngredientTips",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientTips_IngredientId",
                table: "IngredientTips",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientTips_Ingredients_IngredientId",
                table: "IngredientTips",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRecipeFieldsForBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Recipes",
                newName: "RecipePrice");

            migrationBuilder.AddColumn<double>(
                name: "BookingPrice",
                table: "Recipes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForBooking",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingPrice",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsAvailableForBooking",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "RecipePrice",
                table: "Recipes",
                newName: "Price");
        }
    }
}

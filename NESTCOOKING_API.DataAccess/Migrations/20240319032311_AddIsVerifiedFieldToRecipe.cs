using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVerifiedFieldToRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Recipes");
        }
    }
}

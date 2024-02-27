using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRatingsToDifficult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "2c01c25c-11b6-40df-a60e-d8bf59beb411");

            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "42996687-e4a0-4168-b03d-bd09a7534773");

            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "e3fd7581-60e2-40aa-960b-e006586e63b5");

            migrationBuilder.RenameColumn(
                name: "Ratings",
                table: "Recipes",
                newName: "Difficult");

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "Emoji" },
                values: new object[,]
                {
                    { "1b8a8f0c-55e4-44a3-bd61-73aed749e8ae", "like" },
                    { "7a6deca8-eebd-4072-88a4-28dba4dd0424", "favorite" },
                    { "bc8eff56-b281-4209-af6c-a8e83443642a", "haha" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "1b8a8f0c-55e4-44a3-bd61-73aed749e8ae");

            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "7a6deca8-eebd-4072-88a4-28dba4dd0424");

            migrationBuilder.DeleteData(
                table: "Reactions",
                keyColumn: "Id",
                keyValue: "bc8eff56-b281-4209-af6c-a8e83443642a");

            migrationBuilder.RenameColumn(
                name: "Difficult",
                table: "Recipes",
                newName: "Ratings");

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "Emoji" },
                values: new object[,]
                {
                    { "2c01c25c-11b6-40df-a60e-d8bf59beb411", "like" },
                    { "42996687-e4a0-4168-b03d-bd09a7534773", "haha" },
                    { "e3fd7581-60e2-40aa-960b-e006586e63b5", "favorite" }
                });
        }
    }
}

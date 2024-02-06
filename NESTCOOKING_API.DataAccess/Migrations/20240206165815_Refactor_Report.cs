using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Report : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_Target",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "Reports",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_Target",
                table: "Reports",
                newName: "IX_Reports_TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_TargetId",
                table: "Reports",
                column: "TargetId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_TargetId",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "Reports",
                newName: "Target");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_TargetId",
                table: "Reports",
                newName: "IX_Reports_Target");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_Target",
                table: "Reports",
                column: "Target",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

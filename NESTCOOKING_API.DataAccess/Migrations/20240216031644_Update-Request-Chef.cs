using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestChef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "RequestToBecomeChefs",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Reasons",
                table: "RequestToBecomeChefs",
                newName: "IdentityImageUrl");

            migrationBuilder.RenameColumn(
                name: "IdentityImage",
                table: "RequestToBecomeChefs",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "AchievementImageUrls",
                table: "RequestToBecomeChefs",
                newName: "Experience");

            migrationBuilder.RenameColumn(
                name: "AchievementDescriptions",
                table: "RequestToBecomeChefs",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Achievement",
                table: "RequestToBecomeChefs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "RequestToBecomeChefs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "RequestToBecomeChefs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CertificateImageUrls",
                table: "RequestToBecomeChefs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "RequestToBecomeChefs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achievement",
                table: "RequestToBecomeChefs");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "RequestToBecomeChefs");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "RequestToBecomeChefs");

            migrationBuilder.DropColumn(
                name: "CertificateImageUrls",
                table: "RequestToBecomeChefs");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "RequestToBecomeChefs");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "RequestToBecomeChefs",
                newName: "Skills");

            migrationBuilder.RenameColumn(
                name: "IdentityImageUrl",
                table: "RequestToBecomeChefs",
                newName: "Reasons");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "RequestToBecomeChefs",
                newName: "IdentityImage");

            migrationBuilder.RenameColumn(
                name: "Experience",
                table: "RequestToBecomeChefs",
                newName: "AchievementImageUrls");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "RequestToBecomeChefs",
                newName: "AchievementDescriptions");
        }
    }
}

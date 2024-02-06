using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChefDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestToBecomeChefs",
                columns: table => new
                {
                    RequestChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdentityImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reasons = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchievementImageUrls = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AchievementDescriptions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ResponseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestToBecomeChefs", x => x.RequestChefId);
                    table.ForeignKey(
                        name: "FK_RequestToBecomeChefs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestToBecomeChefs_UserID",
                table: "RequestToBecomeChefs",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestToBecomeChefs");
        }
    }
}

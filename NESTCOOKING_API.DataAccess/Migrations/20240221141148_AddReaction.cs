using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeReaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecipeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeReaction_Reaction_ReactionId",
                        column: x => x.ReactionId,
                        principalTable: "Reaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeReaction_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeReaction_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Reaction",
                columns: new[] { "Id", "CreatedAt", "Emoji", "UpdatedAt" },
                values: new object[,]
                {
                    { "3a285380-2754-4d3a-b1a3-daf91906b41c", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5526), "like", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5539) },
                    { "3c1934cf-37ac-42de-a38a-a36d6713b8d7", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5601), "haha", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5602) },
                    { "ec785843-c674-4e1b-b51f-eb3d2192647e", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5581), "favorite", new DateTime(2024, 2, 21, 21, 11, 48, 158, DateTimeKind.Local).AddTicks(5582) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeReaction_ReactionId",
                table: "RecipeReaction",
                column: "ReactionId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeReaction_RecipeId",
                table: "RecipeReaction",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeReaction_UserId",
                table: "RecipeReaction",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeReaction");

            migrationBuilder.DropTable(
                name: "Reaction");
        }
    }
}

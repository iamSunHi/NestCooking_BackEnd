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
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                columns: new[] { "Id", "Emoji" },
                values: new object[,]
                {
                    { "0da3fad8-2932-4212-b7b7-44e1418154c1", "haha" },
                    { "292bc520-b323-4bf2-8fe5-c77d8d57bf83", "like" },
                    { "5b7cacbd-4076-48cb-a3a7-b7b734d3acdc", "favorite" }
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

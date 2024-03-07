using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NESTCOOKING_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStatisticsToDb : Migration
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

            migrationBuilder.CreateTable(
                name: "BookingStatistics",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalOfBooking = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStatistics", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "ChefStatistics",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfNewChef = table.Column<int>(type: "int", nullable: false),
                    TotalOfChef = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefStatistics", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "RevenueStatistics",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Revenue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueStatistics", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "UserStatistics",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfNewUser = table.Column<int>(type: "int", nullable: false),
                    TotalOfUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStatistics", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "ViolationStatistics",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalOfViolation = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationStatistics", x => x.Date);
                });

            migrationBuilder.InsertData(
                table: "BookingStatistics",
                columns: new[] { "Date", "TotalOfBooking" },
                values: new object[] { new DateOnly(2024, 3, 6), 0 });

            migrationBuilder.InsertData(
                table: "ChefStatistics",
                columns: new[] { "Date", "NumberOfNewChef", "TotalOfChef" },
                values: new object[] { new DateOnly(2024, 3, 6), 0, 0 });

            migrationBuilder.InsertData(
                table: "RevenueStatistics",
                columns: new[] { "Date", "Revenue" },
                values: new object[] { new DateOnly(2024, 3, 6), 0.0 });

            migrationBuilder.InsertData(
                table: "UserStatistics",
                columns: new[] { "Date", "NumberOfNewUser", "TotalOfUser" },
                values: new object[] { new DateOnly(2024, 3, 6), 0, 0 });

            migrationBuilder.InsertData(
                table: "ViolationStatistics",
                columns: new[] { "Date", "TotalOfViolation" },
                values: new object[] { new DateOnly(2024, 3, 6), 0 });

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

            migrationBuilder.DropTable(
                name: "BookingStatistics");

            migrationBuilder.DropTable(
                name: "ChefStatistics");

            migrationBuilder.DropTable(
                name: "RevenueStatistics");

            migrationBuilder.DropTable(
                name: "UserStatistics");

            migrationBuilder.DropTable(
                name: "ViolationStatistics");

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

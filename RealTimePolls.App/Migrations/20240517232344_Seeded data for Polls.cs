using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class SeededdataforPolls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Polls",
                columns: new[] { "Id", "FirstOption", "GenreId", "SecondOption", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Chicken", 13, "Egg", "Which came first?", 1 },
                    { 2, "First choice", 13, "Second choice", "What's your option?", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

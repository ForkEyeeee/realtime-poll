using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class SeededGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Technology" },
                    { 2, "Science" },
                    { 3, "Health & Wellness" },
                    { 4, "Sports" },
                    { 5, "Music" },
                    { 6, "Literature" },
                    { 7, "Travel" },
                    { 8, "Food & Cooking" },
                    { 9, "Fashion" },
                    { 10, "Art & Design" },
                    { 11, "Gaming" },
                    { 12, "Education" },
                    { 13, "Anime" },
                    { 14, "Environment" },
                    { 15, "Business & Finance" },
                    { 16, "Movies & TV" },
                    { 17, "Comedy" },
                    { 18, "Lifestyle" },
                    { 19, "History" },
                    { 20, "DIY & Crafts" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Genre",
                keyColumn: "id",
                keyValue: 20);
        }
    }
}

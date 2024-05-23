using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedSeeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "GoogleId", "Name", "ProfilePicture" },
                values: new object[] { "user1@gmail.com", "1111111", "User One", "https://fastly.picsum.photos/id/1011/5000/3333.jpg?hmac=7dZBmDddTg4Y1HX5N8lZg1b78F3TcD9FZ5ZuBcG1bZc" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "GoogleId", "Name", "ProfilePicture" },
                values: new object[,]
                {
                    { 2, "user2@gmail.com", "2222222", "User Two", "https://fastly.picsum.photos/id/1027/5000/3333.jpg?hmac=Zlkm7by_Afr8RFdY1MZAnL9m8oxfGkl_A5oOIdD_vBc" },
                    { 3, "user3@gmail.com", "3333333", "User Three", "https://fastly.picsum.photos/id/1029/5000/3333.jpg?hmac=oi5G1bcGJ-8KOc7Zt9I1F7vU1g2nDjVljqzM0ElF_Qk" }
                });

            migrationBuilder.InsertData(
                table: "Polls",
                columns: new[] { "Id", "FirstOption", "GenreId", "SecondOption", "Title", "UserId" },
                values: new object[,]
                {
                    { 3, "Summer", 18, "Winter", "Which season do you prefer?", 2 },
                    { 4, "Apple", 1, "Android", "Which smartphone brand do you prefer?", 2 },
                    { 5, "Pizza", 8, "Burger", "Favorite fast food?", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "GoogleId", "Name", "ProfilePicture" },
                values: new object[] { "user@gmail.com", "9999999", "Anonymous User", "https://fastly.picsum.photos/id/0/5000/3333.jpg?hmac=_j6ghY5fCfSD6tvtcV74zXivkJSPIfR9B8w34XeQmvU" });
        }
    }
}

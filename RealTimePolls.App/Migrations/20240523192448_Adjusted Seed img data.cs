using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedSeedimgdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProfilePicture",
                value: "https://picsum.photos/500");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProfilePicture",
                value: "https://picsum.photos/500");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProfilePicture",
                value: "https://picsum.photos/500");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProfilePicture",
                value: "https://fastly.picsum.photos/id/1011/5000/3333.jpg?hmac=7dZBmDddTg4Y1HX5N8lZg1b78F3TcD9FZ5ZuBcG1bZc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProfilePicture",
                value: "https://fastly.picsum.photos/id/1027/5000/3333.jpg?hmac=Zlkm7by_Afr8RFdY1MZAnL9m8oxfGkl_A5oOIdD_vBc");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProfilePicture",
                value: "https://fastly.picsum.photos/id/1029/5000/3333.jpg?hmac=oi5G1bcGJ-8KOc7Zt9I1F7vU1g2nDjVljqzM0ElF_Qk");
        }
    }
}

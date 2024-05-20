using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class Revisedseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name", "ProfilePicture" },
                values: new object[] { "user@gmail.com", "Anonymous User", "https://fastly.picsum.photos/id/0/5000/3333.jpg?hmac=_j6ghY5fCfSD6tvtcV74zXivkJSPIfR9B8w34XeQmvU" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Name", "ProfilePicture" },
                values: new object[] { "shawncarter123456@gmail.com", "Windows 10", "https://image.png" });
        }
    }
}

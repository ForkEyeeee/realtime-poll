using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations.RealTimePollsAuthDb
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a71a55d6-99d7-4123-b4e0-1218ecb90e3e", "a71a55d6-99d7-4123-b4e0-1218ecb90e3e", "Reader", "READER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a71a55d6-99d7-4123-b4e0-1218ecb90e3e");
        }
    }
}

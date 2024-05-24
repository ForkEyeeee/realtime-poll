using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations.RealTimePollsAuthDb
{
    /// <inheritdoc />
    public partial class ModifiedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a71a55d6-99d7-4123-b4e0-1218ecb90e3e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "USER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a71a55d6-99d7-4123-b4e0-1218ecb90e3e",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Reader", "READER" });
        }
    }
}

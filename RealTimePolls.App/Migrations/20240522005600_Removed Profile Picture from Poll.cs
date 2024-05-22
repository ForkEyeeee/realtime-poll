using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class RemovedProfilePicturefromPoll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 1,
                column: "GenreId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 2,
                column: "GenreId",
                value: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 1,
                column: "GenreId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Polls",
                keyColumn: "Id",
                keyValue: 2,
                column: "GenreId",
                value: 5);
        }
    }
}

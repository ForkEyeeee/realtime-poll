using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class AddedGenretoPoll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Polls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_GenreId",
                table: "Polls",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Genre_GenreId",
                table: "Polls",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Genre_GenreId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_GenreId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Polls");
        }
    }
}

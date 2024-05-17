using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class Changedcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Genre_genreid",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_genreid",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "genreid",
                table: "Polls");

            migrationBuilder.RenameColumn(
                name: "vote",
                table: "UserPoll",
                newName: "Vote");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "UserPoll",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "pollid",
                table: "UserPoll",
                newName: "PollId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserPoll",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "profilepicture",
                table: "User",
                newName: "ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "User",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "googleid",
                table: "User",
                newName: "GoogleId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "Polls",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Polls",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "secondoption",
                table: "Polls",
                newName: "SecondOption");

            migrationBuilder.RenameColumn(
                name: "firstoption",
                table: "Polls",
                newName: "FirstOption");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Polls",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Genre",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Genre",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vote",
                table: "UserPoll",
                newName: "vote");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserPoll",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "PollId",
                table: "UserPoll",
                newName: "pollid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserPoll",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "User",
                newName: "profilepicture");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "GoogleId",
                table: "User",
                newName: "googleid");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Polls",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Polls",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "SecondOption",
                table: "Polls",
                newName: "secondoption");

            migrationBuilder.RenameColumn(
                name: "FirstOption",
                table: "Polls",
                newName: "firstoption");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Polls",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Genre",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Genre",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "profilepicture",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "googleid",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "genreid",
                table: "Polls",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_genreid",
                table: "Polls",
                column: "genreid");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Genre_genreid",
                table: "Polls",
                column: "genreid",
                principalTable: "Genre",
                principalColumn: "id");
        }
    }
}

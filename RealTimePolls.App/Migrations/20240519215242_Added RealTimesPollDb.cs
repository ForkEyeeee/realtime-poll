using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class AddedRealTimesPollDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPoll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PollId = table.Column<int>(type: "int", nullable: false),
                    Vote = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPoll", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Polls_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genre",
                columns: new[] { "Id", "Name" },
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

            migrationBuilder.InsertData(
                table: "Polls",
                columns: new[] { "Id", "FirstOption", "GenreId", "SecondOption", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Chicken", 13, "Egg", "Which came first?", 1 },
                    { 2, "First choice", 13, "Second choice", "What's your option?", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polls_GenreId",
                table: "Polls",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_UserId",
                table: "Polls",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "UserPoll");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}

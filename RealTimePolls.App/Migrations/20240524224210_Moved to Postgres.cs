using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealTimePolls.Migrations
{
    /// <inheritdoc />
    public partial class MovedtoPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GoogleId = table.Column<string>(type: "text", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPoll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PollId = table.Column<int>(type: "integer", nullable: false),
                    Vote = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPoll", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FirstOption = table.Column<string>(type: "text", nullable: false),
                    SecondOption = table.Column<string>(type: "text", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
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
                table: "User",
                columns: new[] { "Id", "Email", "GoogleId", "Name", "ProfilePicture" },
                values: new object[,]
                {
                    { 1, "user1@gmail.com", "1111111", "User One", "https://picsum.photos/500" },
                    { 2, "user2@gmail.com", "2222222", "User Two", "https://picsum.photos/500" },
                    { 3, "user3@gmail.com", "3333333", "User Three", "https://picsum.photos/500" }
                });

            migrationBuilder.InsertData(
                table: "Polls",
                columns: new[] { "Id", "FirstOption", "GenreId", "SecondOption", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, "Chicken", 4, "Egg", "Which came first?", 1 },
                    { 2, "First choice", 6, "Second choice", "What is your option?", 1 },
                    { 3, "Summer", 18, "Winter", "Which season do you prefer?", 2 },
                    { 4, "Apple", 1, "Android", "Which smartphone brand do you prefer?", 2 },
                    { 5, "Pizza", 8, "Burger", "Favorite fast food?", 3 }
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

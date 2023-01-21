using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayhem.Dal.Migrations
{
    /// <inheritdoc />
    public partial class NewTableVoteCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteCategoryId",
                table: "GameUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VoteCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotePower = table.Column<int>(type: "int", nullable: false),
                    InvestorCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameUser_VoteCategoryId",
                table: "GameUser",
                column: "VoteCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_VoteCategories_VoteCategoryId",
                table: "GameUser",
                column: "VoteCategoryId",
                principalTable: "VoteCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_VoteCategories_VoteCategoryId",
                table: "GameUser");

            migrationBuilder.DropTable(
                name: "VoteCategories");

            migrationBuilder.DropIndex(
                name: "IX_GameUser_VoteCategoryId",
                table: "GameUser");

            migrationBuilder.DropColumn(
                name: "VoteCategoryId",
                table: "GameUser");
        }
    }
}

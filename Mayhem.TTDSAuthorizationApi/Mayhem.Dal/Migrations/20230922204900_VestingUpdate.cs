using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayhem.Dal.Migrations
{
    /// <inheritdoc />
    public partial class VestingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestorCategory",
                table: "VoteCategories");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "VoteCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UsdcAmount",
                table: "GameUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "VoteCategories");

            migrationBuilder.DropColumn(
                name: "UsdcAmount",
                table: "GameUser");

            migrationBuilder.AddColumn<int>(
                name: "InvestorCategory",
                table: "VoteCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

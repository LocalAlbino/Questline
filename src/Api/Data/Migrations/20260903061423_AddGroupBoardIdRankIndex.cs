using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Questline.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupBoardIdRankIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_BoardId",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BoardId_Rank",
                table: "Groups",
                columns: new[] { "BoardId", "Rank" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_BoardId_Rank",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BoardId",
                table: "Groups",
                column: "BoardId");
        }
    }
}

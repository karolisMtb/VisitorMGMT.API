using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorMGMT.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changedOnModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Visitors_ProfileId",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Profiles",
                newName: "VisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_ProfileId",
                table: "Profiles",
                newName: "IX_Profiles_VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Visitors_VisitorId",
                table: "Profiles",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Visitors_VisitorId",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "VisitorId",
                table: "Profiles",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Profiles_VisitorId",
                table: "Profiles",
                newName: "IX_Profiles_ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Visitors_ProfileId",
                table: "Profiles",
                column: "ProfileId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

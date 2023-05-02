using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorMGMT.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditedProfileImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImages_Profiles_Id",
                table: "ProfileImages");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImages_ProfileId",
                table: "ProfileImages",
                column: "ProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImages_Profiles_ProfileId",
                table: "ProfileImages",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImages_Profiles_ProfileId",
                table: "ProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_ProfileImages_ProfileId",
                table: "ProfileImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImages_Profiles_Id",
                table: "ProfileImages",
                column: "Id",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

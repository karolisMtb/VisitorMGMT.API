using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisitorMGMT.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Profiles_ProfileId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_ProfileId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "VisitorId",
                table: "Profiles",
                newName: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ProfileId",
                table: "Profiles",
                column: "ProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Visitors_ProfileId",
                table: "Profiles",
                column: "ProfileId",
                principalTable: "Visitors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Visitors_ProfileId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_ProfileId",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Profiles",
                newName: "VisitorId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Visitors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_ProfileId",
                table: "Visitors",
                column: "ProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Profiles_ProfileId",
                table: "Visitors",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

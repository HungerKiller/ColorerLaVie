using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoMasterBackend.Migrations
{
    public partial class AddColumnPathIntoPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Photos_PhotoId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_PhotoId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Labels");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PhotoLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(type: "int", nullable: false),
                    LabelId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoLabels_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoLabels_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhotoLabels_LabelId",
                table: "PhotoLabels",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoLabels_PhotoId",
                table: "PhotoLabels",
                column: "PhotoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoLabels");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Labels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Labels_PhotoId",
                table: "Labels",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Photos_PhotoId",
                table: "Labels",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

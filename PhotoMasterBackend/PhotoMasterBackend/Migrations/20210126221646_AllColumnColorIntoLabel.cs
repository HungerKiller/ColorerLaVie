using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoMasterBackend.Migrations
{
    public partial class AllColumnColorIntoLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Labels");
        }
    }
}

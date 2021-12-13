using Microsoft.EntityFrameworkCore.Migrations;

namespace OpsMain.StorageLayer.Migrations
{
    public partial class AddParentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "SysMenu",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "SysMenu");
        }
    }
}

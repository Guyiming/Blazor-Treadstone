using Microsoft.EntityFrameworkCore.Migrations;

namespace OpsMain.StorageLayer.Migrations
{
    public partial class AddSysMenForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "SysMenu",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenu_ParentId",
                table: "SysMenu",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SysMenu_SysMenu_ParentId",
                table: "SysMenu",
                column: "ParentId",
                principalTable: "SysMenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysMenu_SysMenu_ParentId",
                table: "SysMenu");

            migrationBuilder.DropIndex(
                name: "IX_SysMenu_ParentId",
                table: "SysMenu");

            migrationBuilder.AlterColumn<long>(
                name: "ParentId",
                table: "SysMenu",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}

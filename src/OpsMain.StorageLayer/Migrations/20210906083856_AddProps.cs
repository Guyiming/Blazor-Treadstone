using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpsMain.StorageLayer.Migrations
{
    public partial class AddProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysMenu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "菜单名称"),
                    Href = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "菜单Url"),
                    Icon = table.Column<string>(type: "nvarchar(1100)", maxLength: 1100, nullable: true, comment: "菜单图标"),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    OrderNo = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "角色名称"),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "用户名"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuSysRole",
                columns: table => new
                {
                    MenusId = table.Column<long>(type: "bigint", nullable: false),
                    RolesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuSysRole", x => new { x.MenusId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_SysMenuSysRole_SysMenu_MenusId",
                        column: x => x.MenusId,
                        principalTable: "SysMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysMenuSysRole_SysRole_RolesId",
                        column: x => x.RolesId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleSysUser",
                columns: table => new
                {
                    RolesId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleSysUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SysRoleSysUser_SysRole_RolesId",
                        column: x => x.RolesId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysRoleSysUser_SysUser_UsersId",
                        column: x => x.UsersId,
                        principalTable: "SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuSysRole_RolesId",
                table: "SysMenuSysRole",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRoleSysUser_UsersId",
                table: "SysRoleSysUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SysUser_UserName",
                table: "SysUser",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysMenuSysRole");

            migrationBuilder.DropTable(
                name: "SysRoleSysUser");

            migrationBuilder.DropTable(
                name: "SysMenu");

            migrationBuilder.DropTable(
                name: "SysRole");

            migrationBuilder.DropTable(
                name: "SysUser");
        }
    }
}

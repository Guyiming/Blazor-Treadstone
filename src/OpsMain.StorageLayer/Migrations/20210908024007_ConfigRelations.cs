using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpsMain.StorageLayer.Migrations
{
    public partial class ConfigRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysMenuSysRole");

            migrationBuilder.DropTable(
                name: "SysRoleSysUser");

            migrationBuilder.CreateTable(
                name: "R_RoleMenu",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    MenuId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_RoleMenu", x => new { x.RoleId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_R_RoleMenu_SysMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "SysMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_R_RoleMenu_SysRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "R_RoleUser",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_RoleUser", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_R_RoleUser_SysRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_R_RoleUser_SysUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_R_RoleMenu_MenuId",
                table: "R_RoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_R_RoleUser_UserId",
                table: "R_RoleUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "R_RoleMenu");

            migrationBuilder.DropTable(
                name: "R_RoleUser");

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
        }
    }
}

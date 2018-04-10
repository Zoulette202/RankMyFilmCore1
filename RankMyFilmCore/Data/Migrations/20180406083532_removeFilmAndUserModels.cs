using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class removeFilmAndUserModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filmModel");

            migrationBuilder.DropTable(
                name: "userModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "filmModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filmModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "userModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    ConfirmPassword = table.Column<string>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_userModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userModel_UserId",
                table: "userModel",
                column: "UserId");
        }
    }
}

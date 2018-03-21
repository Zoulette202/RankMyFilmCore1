using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idGuidAspNetUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userModel_UserId",
                table: "userModel");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_guid",
                table: "AspNetUsers",
                column: "guid");

            migrationBuilder.CreateIndex(
                name: "IX_userModel_UserId",
                table: "userModel",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userModel_UserId",
                table: "userModel");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_guid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "guid",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_userModel_UserId",
                table: "userModel",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}

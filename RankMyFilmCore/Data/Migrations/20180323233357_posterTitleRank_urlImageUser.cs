using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class posterTitleRank_urlImageUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_guid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "guid",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "rankModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "poster",
                table: "rankModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "urlImage",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "rankModel");

            migrationBuilder.DropColumn(
                name: "poster",
                table: "rankModel");

            migrationBuilder.DropColumn(
                name: "urlImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_guid",
                table: "AspNetUsers",
                column: "guid");
        }
    }
}

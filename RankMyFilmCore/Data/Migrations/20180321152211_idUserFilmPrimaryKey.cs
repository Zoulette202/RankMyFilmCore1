using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idUserFilmPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userModel_UserId",
                table: "userModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel");

            migrationBuilder.AlterColumn<string>(
                name: "idUser",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "idFilm",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_rankModel_ID",
                table: "rankModel",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel",
                columns: new[] { "idUser", "idFilm" });

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
                name: "AK_rankModel_ID",
                table: "rankModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_guid",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "guid",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "idFilm",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "idUser",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_userModel_UserId",
                table: "userModel",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}

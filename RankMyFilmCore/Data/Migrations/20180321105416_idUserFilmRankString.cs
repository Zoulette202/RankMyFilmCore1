using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idUserFilmRankString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "userModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "userModel",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "userModel",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "idUser",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "idFilm",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "userModel");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "userModel");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "userModel");

            migrationBuilder.AlterColumn<int>(
                name: "idUser",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idFilm",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

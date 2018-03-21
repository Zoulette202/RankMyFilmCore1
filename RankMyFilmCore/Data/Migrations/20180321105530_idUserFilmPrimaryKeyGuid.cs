using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idUserFilmPrimaryKeyGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "idUser",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "idFilm",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_rankModel_ID",
                table: "rankModel",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel",
                columns: new[] { "idUser", "idFilm" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_rankModel_ID",
                table: "rankModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel");

            migrationBuilder.AlterColumn<string>(
                name: "idFilm",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "idUser",
                table: "rankModel",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_rankModel",
                table: "rankModel",
                column: "ID");
        }
    }
}

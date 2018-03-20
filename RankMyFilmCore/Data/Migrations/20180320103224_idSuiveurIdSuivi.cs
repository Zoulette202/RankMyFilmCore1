using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idSuiveurIdSuivi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idUserFirst",
                table: "friendsModel");

            migrationBuilder.RenameColumn(
                name: "idUserSecond",
                table: "friendsModel",
                newName: "idSuivi");

            migrationBuilder.AddColumn<int>(
                name: "idSuiveur",
                table: "friendsModel",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idSuiveur",
                table: "friendsModel");

            migrationBuilder.RenameColumn(
                name: "idSuivi",
                table: "friendsModel",
                newName: "idUserSecond");

            migrationBuilder.AddColumn<int>(
                name: "idUserFirst",
                table: "friendsModel",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }
    }
}

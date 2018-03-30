using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idSuiveurSuiviFriendPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_friendsModel",
                table: "friendsModel");

            migrationBuilder.AlterColumn<string>(
                name: "idSuivi",
                table: "friendsModel",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "idSuiveur",
                table: "friendsModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_friendsModel",
                table: "friendsModel",
                columns: new[] { "idSuiveur", "idSuivi" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_friendsModel_ID_idSuiveur_idSuivi",
                table: "friendsModel",
                columns: new[] { "ID", "idSuiveur", "idSuivi" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_friendsModel",
                table: "friendsModel");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_friendsModel_ID_idSuiveur_idSuivi",
                table: "friendsModel");

            migrationBuilder.AlterColumn<string>(
                name: "idSuivi",
                table: "friendsModel",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "idSuiveur",
                table: "friendsModel",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_friendsModel",
                table: "friendsModel",
                column: "ID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class idStringFriendsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_rankModel_ID",
                table: "rankModel");

            migrationBuilder.AlterColumn<string>(
                name: "idUser",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "idSuivi",
                table: "friendsModel",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "idSuiveur",
                table: "friendsModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "idFriends",
                table: "friendsModel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_rankModel_ID_idFilm_idUser",
                table: "rankModel",
                columns: new[] { "ID", "idFilm", "idUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_rankModel_ID_idFilm_idUser",
                table: "rankModel");

            migrationBuilder.AlterColumn<Guid>(
                name: "idUser",
                table: "rankModel",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "idSuivi",
                table: "friendsModel",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idSuiveur",
                table: "friendsModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "idFriends",
                table: "friendsModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_rankModel_ID",
                table: "rankModel",
                column: "ID");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class Models1603 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "filmModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filmModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "friendsModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    idFriends = table.Column<int>(maxLength: 50, nullable: false),
                    idUserFirst = table.Column<int>(maxLength: 50, nullable: false),
                    idUserSecond = table.Column<int>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_friendsModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "rankModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Commentaire = table.Column<string>(maxLength: 255, nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Vote = table.Column<int>(maxLength: 1, nullable: false),
                    idFilm = table.Column<int>(maxLength: 50, nullable: false),
                    idUser = table.Column<int>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rankModel", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filmModel");

            migrationBuilder.DropTable(
                name: "friendsModel");

            migrationBuilder.DropTable(
                name: "rankModel");
        }
    }
}

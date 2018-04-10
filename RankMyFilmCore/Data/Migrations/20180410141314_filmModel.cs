using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RankMyFilmCore.Data.Migrations
{
    public partial class filmModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "rankModel");

            migrationBuilder.DropColumn(
                name: "poster",
                table: "rankModel");

            migrationBuilder.CreateTable(
                name: "filmModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    idFilm = table.Column<string>(nullable: true),
                    moyenne = table.Column<double>(nullable: false),
                    nbRank = table.Column<int>(nullable: false),
                    poster = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_filmModel", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "filmModel");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "rankModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "poster",
                table: "rankModel",
                nullable: true);
        }
    }
}

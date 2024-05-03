using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessMaster.Infrastructure.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Salt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password_Hash = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts_UserId", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Creator_User_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Creation_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    White_Player_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Black_Player_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Start_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    End_Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Winner_Id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyBooks.Migrations
{
    /// <inheritdoc />
    public partial class AddedLibraryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "LibraryId",
                table: "Books",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PublicId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libraries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_LibraryId",
                table: "Books",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_UserId",
                table: "Libraries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Libraries_LibraryId",
                table: "Books",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Libraries_LibraryId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Books_LibraryId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "LibraryId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "Isbn",
                table: "Books",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddnewBookProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Books",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseYear",
                table: "Books",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Books");
        }
    }
}

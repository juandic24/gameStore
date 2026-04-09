using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platforms",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Platforms",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Games");
        }
    }
}

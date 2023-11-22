using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tusur.Ais.Migrations
{
    public partial class migr1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Contracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DirectorLastName",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DirectorName",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DirectorPatronymic",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectorLastName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DirectorName",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DirectorPatronymic",
                table: "Companies");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Contracts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

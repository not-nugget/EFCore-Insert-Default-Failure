using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UUID",
                table: "Entities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "UUID",
                table: "Entities",
                type: "binary(16)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}

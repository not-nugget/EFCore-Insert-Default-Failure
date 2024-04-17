using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scratch.Migrations
{
    /// <inheritdoc />
    public partial class BintoVarchar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UUID",
                table: "Entities",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "binary(16)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Entities",
                type: "varchar(255)",
                nullable: false,
                defaultValueSql: "(UUID())",
                oldClrType: typeof(byte[]),
                oldType: "binary(16)",
                oldDefaultValueSql: "(UUID_TO_BIN(UUID(), 1))")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "UUID",
                table: "Entities",
                type: "binary(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Id",
                table: "Entities",
                type: "binary(16)",
                nullable: false,
                defaultValueSql: "(UUID_TO_BIN(UUID(), 1))",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldDefaultValueSql: "(UUID())")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}

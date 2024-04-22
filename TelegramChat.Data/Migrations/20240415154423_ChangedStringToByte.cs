using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramChat.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStringToByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""MessageHistories"" ALTER COLUMN ""Text"" TYPE BYTEA USING ""Text""::bytea");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "MessageHistories",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "bytea");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGen23.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIssuerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuerName",
                table: "Card");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuerName",
                table: "Card",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}

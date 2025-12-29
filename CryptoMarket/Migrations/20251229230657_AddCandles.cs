using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMarket.Migrations
{
    /// <inheritdoc />
    public partial class AddCandles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "Prices",
                newName: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Prices",
                newName: "TimeStamp");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ewan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyHourlyPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerHour",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Properties");
        }
    }
}

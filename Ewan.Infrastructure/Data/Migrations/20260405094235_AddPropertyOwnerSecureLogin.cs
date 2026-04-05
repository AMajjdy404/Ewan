using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ewan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyOwnerSecureLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerPasswordHash",
                table: "Properties",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerPhoneNumber",
                table: "Properties",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerPhoneNumber",
                table: "Properties",
                column: "OwnerPhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_OwnerPhoneNumber",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "OwnerPasswordHash",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "OwnerPhoneNumber",
                table: "Properties");
        }
    }
}

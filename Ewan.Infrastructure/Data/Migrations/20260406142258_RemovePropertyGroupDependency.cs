using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ewan.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertyGroupDependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyGroups_GroupId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "PropertyGroups");

            migrationBuilder.DropIndex(
                name: "IX_Properties_GroupId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Properties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PropertyGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_GroupId",
                table: "Properties",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyGroups_GroupId",
                table: "Properties",
                column: "GroupId",
                principalTable: "PropertyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Isdeleted",
                table: "Category",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Category",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Order",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Category",
                newName: "Isdeleted");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Category",
                newName: "Decription");
        }
    }
}

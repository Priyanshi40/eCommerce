using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderProduct",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OrderProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OrderProduct",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "OrderProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "OrderProduct");
        }
    }
}

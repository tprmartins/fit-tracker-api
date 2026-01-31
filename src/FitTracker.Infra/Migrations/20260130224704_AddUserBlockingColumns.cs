using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBlockingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BlockedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BlockedBy",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BlockedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");
        }
    }
}

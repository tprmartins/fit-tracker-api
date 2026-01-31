using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitTracker.Infra.Migrations
{
    /// <inheritdoc />
    public partial class FixDefaultTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"Users\" SET \"CreatedAt\" = NOW() WHERE \"CreatedAt\" = '0001-01-01 00:00:00+00';");
            migrationBuilder.Sql("UPDATE \"Workouts\" SET \"CreatedAt\" = NOW() WHERE \"CreatedAt\" = '0001-01-01 00:00:00+00';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

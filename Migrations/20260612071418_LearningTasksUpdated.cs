using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace traineeManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class LearningTasksUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 12, 7, 14, 18, 321, DateTimeKind.Utc).AddTicks(6803), new DateTime(2026, 6, 12, 7, 14, 18, 321, DateTimeKind.Utc).AddTicks(7008) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 6, 12, 6, 54, 43, 294, DateTimeKind.Utc).AddTicks(8220), new DateTime(2026, 6, 12, 6, 54, 43, 294, DateTimeKind.Utc).AddTicks(8633) });
        }
    }
}

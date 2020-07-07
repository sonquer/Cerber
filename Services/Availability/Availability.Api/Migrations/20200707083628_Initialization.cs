using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Availability.Api.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "availability");

            migrationBuilder.CreateTable(
                name: "availability_records",
                schema: "availability",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    account_id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    expected_status_code = table.Column<int>(nullable: false),
                    expected_response = table.Column<string>(nullable: true),
                    log_lifetime_threshold_in_hours = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_availability_records_id", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "availability_logs",
                schema: "availability",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    status_code = table.Column<int>(nullable: false),
                    body = table.Column<string>(nullable: true),
                    response_time = table.Column<long>(nullable: false),
                    availability_record_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_availability_logs_id", x => x.id);
                    table.ForeignKey(
                        name: "fk_availability_records_availability_logs",
                        column: x => x.availability_record_id,
                        principalSchema: "availability",
                        principalTable: "availability_records",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_availability_logs_availability_record_id",
                schema: "availability",
                table: "availability_logs",
                column: "availability_record_id");

            migrationBuilder.CreateIndex(
                name: "ix_availiability_recrods_account_id",
                schema: "availability",
                table: "availability_records",
                column: "account_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "availability_logs",
                schema: "availability");

            migrationBuilder.DropTable(
                name: "availability_records",
                schema: "availability");
        }
    }
}

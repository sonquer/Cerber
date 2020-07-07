using Microsoft.EntityFrameworkCore.Migrations;

namespace Availability.Api.Migrations
{
    public partial class Fixed_index_to_not_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_availiability_recrods_account_id",
                schema: "availability",
                table: "availability_records");

            migrationBuilder.CreateIndex(
                name: "ix_availiability_recrods_account_id",
                schema: "availability",
                table: "availability_records",
                column: "account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_availiability_recrods_account_id",
                schema: "availability",
                table: "availability_records");

            migrationBuilder.CreateIndex(
                name: "ix_availiability_recrods_account_id",
                schema: "availability",
                table: "availability_records",
                column: "account_id",
                unique: true);
        }
    }
}

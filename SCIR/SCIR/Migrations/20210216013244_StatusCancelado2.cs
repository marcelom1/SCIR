using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class StatusCancelado2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 95,
                column: "Cancelamento",
                value: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 95,
                column: "Cancelamento",
                value: true);
        }
    }
}

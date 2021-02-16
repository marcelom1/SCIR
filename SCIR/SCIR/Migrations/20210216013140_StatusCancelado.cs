using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class StatusCancelado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StatusRequerimento",
                columns: new[] { "Id", "Ativo", "Cancelamento", "CodigoInterno", "Nome" },
                values: new object[] { 95, true, true, 5, "Cancelado" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 95);
        }
    }
}

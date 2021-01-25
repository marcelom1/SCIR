using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class CadastroStatusRequerimentoPadrao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StatusRequerimento",
                columns: new[] { "Id", "Ativo", "Cancelamento", "CodigoInterno", "Nome" },
                values: new object[,]
                {
                    { 96, true, false, 1, "Deferido" },
                    { 97, true, false, 2, "Indeferido" },
                    { 98, true, true, 3, "Protocolado" },
                    { 99, true, true, 4, "Aguardando Esclarecimento" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "StatusRequerimento",
                keyColumn: "Id",
                keyValue: 99);
        }
    }
}

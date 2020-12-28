using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class fluxoStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "FluxoStatus",
                columns: table => new
                {
                    StatusAtualId = table.Column<int>(nullable: false),
                    StatusProximoId = table.Column<int>(nullable: false),
                    TipoRequerimentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoStatus", x => new { x.StatusAtualId, x.StatusProximoId, x.TipoRequerimentoId });
                    table.ForeignKey(
                        name: "FK_FluxoStatus_StatusRequerimento_StatusAtualId",
                        column: x => x.StatusAtualId,
                        principalTable: "StatusRequerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxoStatus_StatusRequerimento_StatusProximoId",
                        column: x => x.StatusProximoId,
                        principalTable: "StatusRequerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxoStatus_TipoRequerimento_TipoRequerimentoId",
                        column: x => x.TipoRequerimentoId,
                        principalTable: "TipoRequerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FluxoStatus_StatusProximoId",
                table: "FluxoStatus",
                column: "StatusProximoId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoStatus_TipoRequerimentoId",
                table: "FluxoStatus",
                column: "TipoRequerimentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FluxoStatus");

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Ativo", "Email", "Nome", "PapelId", "Senha" },
                values: new object[] { 1, true, "marcelo.miglioli@hotmail.com", "Administrador", 1, "123" });
        }
    }
}

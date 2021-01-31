using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class UsuarioPrimeiroAtendimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimeiroAtendimentoId",
                table: "TipoRequerimento",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TipoRequerimento_PrimeiroAtendimentoId",
                table: "TipoRequerimento",
                column: "PrimeiroAtendimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TipoRequerimento_Usuario_PrimeiroAtendimentoId",
                table: "TipoRequerimento",
                column: "PrimeiroAtendimentoId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TipoRequerimento_Usuario_PrimeiroAtendimentoId",
                table: "TipoRequerimento");

            migrationBuilder.DropIndex(
                name: "IX_TipoRequerimento_PrimeiroAtendimentoId",
                table: "TipoRequerimento");

            migrationBuilder.DropColumn(
                name: "PrimeiroAtendimentoId",
                table: "TipoRequerimento");
        }
    }
}

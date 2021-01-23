using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class ArquivosNomeProtocolo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SequenciaProtocolo",
                table: "TipoRequerimento",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sigla",
                table: "TipoRequerimento",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "ArquivoRequerimento",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenciaProtocolo",
                table: "TipoRequerimento");

            migrationBuilder.DropColumn(
                name: "Sigla",
                table: "TipoRequerimento");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "ArquivoRequerimento");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class TipoValidacaoCurricular : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoValidacaoCurricular",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoValidacaoCurricular", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TipoValidacaoCurricular",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 1, true, "Reconhecimento de Estudos" });

            migrationBuilder.InsertData(
                table: "TipoValidacaoCurricular",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 2, true, "Reconhecimento de Saberes" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoValidacaoCurricular");
        }
    }
}

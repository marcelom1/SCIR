using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class Arquivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArquivoRequerimento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Caminho = table.Column<string>(nullable: true),
                    RequerimentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivoRequerimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivoRequerimento_Requerimento_RequerimentoId",
                        column: x => x.RequerimentoId,
                        principalTable: "Requerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArquivoRequerimento_RequerimentoId",
                table: "ArquivoRequerimento",
                column: "RequerimentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArquivoRequerimento");
        }
    }
}

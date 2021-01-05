using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class HistoricoRequerimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricoRequerimento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Modificado = table.Column<DateTime>(nullable: false),
                    Antes = table.Column<string>(nullable: true),
                    Depois = table.Column<string>(nullable: true),
                    RequerimentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoRequerimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoRequerimento_Requerimento_RequerimentoId",
                        column: x => x.RequerimentoId,
                        principalTable: "Requerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoRequerimento_RequerimentoId",
                table: "HistoricoRequerimento",
                column: "RequerimentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoRequerimento");
        }
    }
}

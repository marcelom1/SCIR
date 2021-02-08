using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class Auditoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataModificacao = table.Column<DateTime>(nullable: false),
                    Campo = table.Column<string>(nullable: true),
                    Antes = table.Column<string>(nullable: true),
                    Depois = table.Column<string>(nullable: true),
                    RequerimentoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditoria_Requerimento_RequerimentoId",
                        column: x => x.RequerimentoId,
                        principalTable: "Requerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_RequerimentoId",
                table: "Auditoria",
                column: "RequerimentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class FormularioValidacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requerimento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Protocolo = table.Column<string>(nullable: true),
                    Abertura = table.Column<DateTime>(nullable: false),
                    Encerramento = table.Column<DateTime>(nullable: false),
                    Mensagem = table.Column<string>(nullable: true),
                    UsuarioRequerenteId = table.Column<int>(nullable: false),
                    UsuarioAtendenteId = table.Column<int>(nullable: false),
                    StatusRequerimentoId = table.Column<int>(nullable: false),
                    TipoRequerimentoId = table.Column<int>(nullable: false),
                    TipoFormularioId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Motivo = table.Column<string>(nullable: true),
                    UnidadeCurricularId = table.Column<int>(nullable: true),
                    TipoValidacaoCurricularId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requerimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requerimento_TipoValidacaoCurricular_TipoValidacaoCurricular~",
                        column: x => x.TipoValidacaoCurricularId,
                        principalTable: "TipoValidacaoCurricular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_UnidadeCurricular_UnidadeCurricularId",
                        column: x => x.UnidadeCurricularId,
                        principalTable: "UnidadeCurricular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_StatusRequerimento_StatusRequerimentoId",
                        column: x => x.StatusRequerimentoId,
                        principalTable: "StatusRequerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_TipoFormulario_TipoFormularioId",
                        column: x => x.TipoFormularioId,
                        principalTable: "TipoFormulario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_TipoRequerimento_TipoRequerimentoId",
                        column: x => x.TipoRequerimentoId,
                        principalTable: "TipoRequerimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_Usuario_UsuarioAtendenteId",
                        column: x => x.UsuarioAtendenteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requerimento_Usuario_UsuarioRequerenteId",
                        column: x => x.UsuarioRequerenteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_TipoValidacaoCurricularId",
                table: "Requerimento",
                column: "TipoValidacaoCurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_UnidadeCurricularId",
                table: "Requerimento",
                column: "UnidadeCurricularId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_StatusRequerimentoId",
                table: "Requerimento",
                column: "StatusRequerimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_TipoFormularioId",
                table: "Requerimento",
                column: "TipoFormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_TipoRequerimentoId",
                table: "Requerimento",
                column: "TipoRequerimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_UsuarioAtendenteId",
                table: "Requerimento",
                column: "UsuarioAtendenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Requerimento_UsuarioRequerenteId",
                table: "Requerimento",
                column: "UsuarioRequerenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requerimento");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class AtualizarUnidadeCurricular : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnidadeCurricular_Cursos_CursoId",
                table: "UnidadeCurricular");

            migrationBuilder.AlterColumn<int>(
                name: "CursoId",
                table: "UnidadeCurricular",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UnidadeCurricular_Cursos_CursoId",
                table: "UnidadeCurricular",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnidadeCurricular_Cursos_CursoId",
                table: "UnidadeCurricular");

            migrationBuilder.AlterColumn<int>(
                name: "CursoId",
                table: "UnidadeCurricular",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UnidadeCurricular_Cursos_CursoId",
                table: "UnidadeCurricular",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class CodigoFormulario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Codigo",
                table: "TipoFormulario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TipoFormulario",
                keyColumn: "Id",
                keyValue: 1,
                column: "Codigo",
                value: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "TipoFormulario");
        }
    }
}

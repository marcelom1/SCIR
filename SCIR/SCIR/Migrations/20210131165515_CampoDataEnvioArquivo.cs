using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class CampoDataEnvioArquivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataEnvio",
                table: "ArquivoRequerimento",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEnvio",
                table: "ArquivoRequerimento");
        }
    }
}

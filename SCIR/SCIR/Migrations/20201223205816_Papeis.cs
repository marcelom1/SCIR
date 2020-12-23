using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCIR.Migrations
{
    public partial class Papeis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Papel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Papel", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Papel",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 1, true, "Administrador" });

            migrationBuilder.InsertData(
                table: "Papel",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 2, true, "Servidor" });

            migrationBuilder.InsertData(
                table: "Papel",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 3, true, "Discente" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Papel");
        }
    }
}

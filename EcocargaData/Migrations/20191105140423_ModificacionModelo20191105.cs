using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20191105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_usuario",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(maxLength: 100, nullable: true),
                    perfil_id = table.Column<int>(nullable: false),
                    compania_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_usuario_data_compania_compania_id",
                        column: x => x.compania_id,
                        principalTable: "data_compania",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_usuario_compania_id",
                table: "data_usuario",
                column: "compania_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_usuario");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20190410 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado_cargador",
                table: "data_cargador");

            migrationBuilder.AddColumn<string>(
                name: "estado_electrolinera_descripcion",
                table: "data_electrolinera",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "disponible_descripcion",
                table: "data_cargador",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codigo_informe_tecnico",
                table: "data_auto",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "data_diccionariotipoconector",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tipo_conector_id = table.Column<int>(nullable: false),
                    tipo_conector_externo = table.Column<string>(maxLength: 50, nullable: true),
                    entidad_externo = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_diccionariotipoconector", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_diccionariotipoconector_data_tipoconector_tipo_conector_id",
                        column: x => x.tipo_conector_id,
                        principalTable: "data_tipoconector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_diccionariotipoconector_tipo_conector_id",
                table: "data_diccionariotipoconector",
                column: "tipo_conector_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_diccionariotipoconector");

            migrationBuilder.DropColumn(
                name: "estado_electrolinera_descripcion",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "disponible_descripcion",
                table: "data_cargador");

            migrationBuilder.DropColumn(
                name: "codigo_informe_tecnico",
                table: "data_auto");

            migrationBuilder.AddColumn<bool>(
                name: "estado_cargador",
                table: "data_cargador",
                nullable: false,
                defaultValue: false);
        }
    }
}

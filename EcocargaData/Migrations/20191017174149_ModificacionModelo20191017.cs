using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20191017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto");

            migrationBuilder.DropForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_unidad_cobro_id",
                table: "data_tipocobro");

            migrationBuilder.DropTable(
                name: "data_unidadcobro");

            migrationBuilder.DropIndex(
                name: "IX_data_tipocobro_unidad_cobro_id",
                table: "data_tipocobro");

            migrationBuilder.DropColumn(
                name: "unidad_cobro_id",
                table: "data_tipocobro");

            migrationBuilder.AddColumn<string>(
                name: "unidad_cobro",
                table: "data_tipocobro",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "id_electrolinera_cliente",
                table: "data_electrolinera",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_electrolinera_sec",
                table: "data_electrolinera",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "id_cargador_cliente",
                table: "data_cargador",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_cargador_sec",
                table: "data_cargador",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "tipo_conector_ac_id",
                table: "data_auto",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto",
                column: "tipo_conector_ac_id",
                principalTable: "data_tipoconector",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto");

            migrationBuilder.DropColumn(
                name: "unidad_cobro",
                table: "data_tipocobro");

            migrationBuilder.DropColumn(
                name: "id_electrolinera_sec",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "id_cargador_sec",
                table: "data_cargador");

            migrationBuilder.AddColumn<int>(
                name: "unidad_cobro_id",
                table: "data_tipocobro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "id_electrolinera_cliente",
                table: "data_electrolinera",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "id_cargador_cliente",
                table: "data_cargador",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "tipo_conector_ac_id",
                table: "data_auto",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "data_unidadcobro",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(maxLength: 100, nullable: true),
                    nombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_unidadcobro", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_tipocobro_unidad_cobro_id",
                table: "data_tipocobro",
                column: "unidad_cobro_id");

            migrationBuilder.AddForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto",
                column: "tipo_conector_ac_id",
                principalTable: "data_tipoconector",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_unidad_cobro_id",
                table: "data_tipocobro",
                column: "unidad_cobro_id",
                principalTable: "data_unidadcobro",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20190930 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_tipo_cobro_id",
                table: "data_tipocobro");

            migrationBuilder.RenameColumn(
                name: "tipo_cobro_id",
                table: "data_tipocobro",
                newName: "unidad_cobro_id");

            migrationBuilder.RenameIndex(
                name: "IX_data_tipocobro_tipo_cobro_id",
                table: "data_tipocobro",
                newName: "IX_data_tipocobro_unidad_cobro_id");

            migrationBuilder.AddColumn<int>(
                name: "cargador_id",
                table: "data_tipocobro",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "estado_electrolinera",
                table: "data_electrolinera",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "id_electrolinera_cliente",
                table: "data_electrolinera",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "modelo",
                table: "data_electrolinera",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "estado_cargador",
                table: "data_cargador",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "id_cargador_cliente",
                table: "data_cargador",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "potencia",
                table: "data_cargador",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_data_tipocobro_cargador_id",
                table: "data_tipocobro",
                column: "cargador_id");

            migrationBuilder.AddForeignKey(
                name: "FK_data_tipocobro_data_cargador_cargador_id",
                table: "data_tipocobro",
                column: "cargador_id",
                principalTable: "data_cargador",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_unidad_cobro_id",
                table: "data_tipocobro",
                column: "unidad_cobro_id",
                principalTable: "data_unidadcobro",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_tipocobro_data_cargador_cargador_id",
                table: "data_tipocobro");

            migrationBuilder.DropForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_unidad_cobro_id",
                table: "data_tipocobro");

            migrationBuilder.DropIndex(
                name: "IX_data_tipocobro_cargador_id",
                table: "data_tipocobro");

            migrationBuilder.DropColumn(
                name: "cargador_id",
                table: "data_tipocobro");

            migrationBuilder.DropColumn(
                name: "estado_electrolinera",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "id_electrolinera_cliente",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "modelo",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "estado_cargador",
                table: "data_cargador");

            migrationBuilder.DropColumn(
                name: "id_cargador_cliente",
                table: "data_cargador");

            migrationBuilder.DropColumn(
                name: "potencia",
                table: "data_cargador");

            migrationBuilder.RenameColumn(
                name: "unidad_cobro_id",
                table: "data_tipocobro",
                newName: "tipo_cobro_id");

            migrationBuilder.RenameIndex(
                name: "IX_data_tipocobro_unidad_cobro_id",
                table: "data_tipocobro",
                newName: "IX_data_tipocobro_tipo_cobro_id");

            migrationBuilder.AddForeignKey(
                name: "FK_data_tipocobro_data_unidadcobro_tipo_cobro_id",
                table: "data_tipocobro",
                column: "tipo_cobro_id",
                principalTable: "data_unidadcobro",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

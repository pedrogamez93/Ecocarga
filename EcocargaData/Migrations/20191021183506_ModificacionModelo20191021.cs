using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20191021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estado_electrolinera_descripcion",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "disponible",
                table: "data_cargador");

            migrationBuilder.DropColumn(
                name: "ocupado",
                table: "data_cargador");

            migrationBuilder.RenameColumn(
                name: "disponible_descripcion",
                table: "data_cargador",
                newName: "estado_cargador");

            migrationBuilder.AlterColumn<string>(
                name: "estado_electrolinera",
                table: "data_electrolinera",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<bool>(
                name: "coordenadas_actualizar",
                table: "data_electrolinera",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "id_estado_electrolinera",
                table: "data_electrolinera",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_estado_cargador",
                table: "data_cargador",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coordenadas_actualizar",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "id_estado_electrolinera",
                table: "data_electrolinera");

            migrationBuilder.DropColumn(
                name: "id_estado_cargador",
                table: "data_cargador");

            migrationBuilder.RenameColumn(
                name: "estado_cargador",
                table: "data_cargador",
                newName: "disponible_descripcion");

            migrationBuilder.AlterColumn<bool>(
                name: "estado_electrolinera",
                table: "data_electrolinera",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "estado_electrolinera_descripcion",
                table: "data_electrolinera",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "disponible",
                table: "data_cargador",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ocupado",
                table: "data_cargador",
                nullable: false,
                defaultValue: false);
        }
    }
}

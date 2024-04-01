using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionNombreElectrolinera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "data_electrolinera",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "rut_propietario",
                table: "data_compania",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "data_electrolinera",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "rut_propietario",
                table: "data_compania",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15);
        }
    }
}

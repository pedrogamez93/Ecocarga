using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModeloCompania : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rut_propietario",
                table: "data_compania",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rut_propietario",
                table: "data_compania");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class ModificacionModelo20191024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto");

            migrationBuilder.AddForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto",
                column: "tipo_conector_ac_id",
                principalTable: "data_tipoconector",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto");

            migrationBuilder.AddForeignKey(
                name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                table: "data_auto",
                column: "tipo_conector_ac_id",
                principalTable: "data_tipoconector",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

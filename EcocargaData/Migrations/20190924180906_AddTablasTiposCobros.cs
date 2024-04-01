using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class AddTablasTiposCobros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "imagen",
                table: "data_marcaauto",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "imagen",
                table: "data_auto",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "data_unidadcobro",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_unidadcobro", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_tipocobro",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    tipo_cobro_id = table.Column<int>(nullable: false),
                    precio = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_tipocobro", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_tipocobro_data_unidadcobro_tipo_cobro_id",
                        column: x => x.tipo_cobro_id,
                        principalTable: "data_unidadcobro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_tipocobro_tipo_cobro_id",
                table: "data_tipocobro",
                column: "tipo_cobro_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_tipocobro");

            migrationBuilder.DropTable(
                name: "data_unidadcobro");

            migrationBuilder.AlterColumn<string>(
                name: "imagen",
                table: "data_marcaauto",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "imagen",
                table: "data_auto",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cl.Gob.Energia.Ecocarga.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "data_compania",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 30, nullable: false),
                    url_in_image = table.Column<string>(maxLength: 200, nullable: true),
                    url_image = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_compania", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_marcaauto",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 30, nullable: false),
                    imagen = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_marcaauto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_tipoconector",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    id_publico = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_tipoconector", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_version",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    version = table.Column<Guid>(nullable: false),
                    timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_version", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "data_electrolinera",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 50, nullable: false),
                    latitud = table.Column<double>(nullable: false),
                    longitud = table.Column<double>(nullable: false),
                    direccion = table.Column<string>(maxLength: 100, nullable: false),
                    cantidad_puntos_carga = table.Column<int>(nullable: false),
                    marca = table.Column<string>(maxLength: 30, nullable: false),
                    potencia = table.Column<double>(nullable: false),
                    precio = table.Column<double>(nullable: false),
                    horario = table.Column<string>(maxLength: 100, nullable: false),
                    comuna = table.Column<string>(maxLength: 50, nullable: false),
                    region = table.Column<string>(maxLength: 50, nullable: false),
                    id_publico = table.Column<Guid>(nullable: false),
                    compania_id = table.Column<int>(nullable: false),
                    acepta_conexion_ac = table.Column<bool>(nullable: false),
                    acepta_conexion_dc = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_electrolinera", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_electrolinera_data_compania_compania_id",
                        column: x => x.compania_id,
                        principalTable: "data_compania",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "data_auto",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    modelo = table.Column<string>(maxLength: 100, nullable: false),
                    traccion = table.Column<string>(maxLength: 10, nullable: false),
                    id_publico = table.Column<Guid>(nullable: false),
                    marca_id = table.Column<int>(nullable: false),
                    tipo_conector_ac_id = table.Column<int>(nullable: true),
                    tipo_conector_dc_id = table.Column<int>(nullable: true),
                    capacidad_bateria = table.Column<double>(nullable: false),
                    capacidad_inversor_interno_ac = table.Column<double>(nullable: false),
                    rendimiento_electrico = table.Column<double>(nullable: false),
                    imagen = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_auto", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_auto_data_marcaauto_marca_id",
                        column: x => x.marca_id,
                        principalTable: "data_marcaauto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_data_auto_data_tipoconector_tipo_conector_ac_id",
                        column: x => x.tipo_conector_ac_id,
                        principalTable: "data_tipoconector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_data_auto_data_tipoconector_tipo_conector_dc_id",
                        column: x => x.tipo_conector_dc_id,
                        principalTable: "data_tipoconector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "data_cargador",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cable = table.Column<bool>(nullable: false),
                    ocupado = table.Column<bool>(nullable: false),
                    electrolinera_id = table.Column<int>(nullable: false),
                    tipo_conector_id = table.Column<int>(nullable: false),
                    disponible = table.Column<bool>(nullable: false),
                    tipo_corriente = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_cargador", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_cargador_data_electrolinera_electrolinera_id",
                        column: x => x.electrolinera_id,
                        principalTable: "data_electrolinera",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_data_cargador_data_tipoconector_tipo_conector_id",
                        column: x => x.tipo_conector_id,
                        principalTable: "data_tipoconector",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "data_observacion",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    mensaje = table.Column<string>(maxLength: 150, nullable: false),
                    electrolinera_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_observacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_data_observacion_data_electrolinera_electrolinera_id",
                        column: x => x.electrolinera_id,
                        principalTable: "data_electrolinera",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_auto_marca_id",
                table: "data_auto",
                column: "marca_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_auto_tipo_conector_ac_id",
                table: "data_auto",
                column: "tipo_conector_ac_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_auto_tipo_conector_dc_id",
                table: "data_auto",
                column: "tipo_conector_dc_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_cargador_electrolinera_id",
                table: "data_cargador",
                column: "electrolinera_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_cargador_tipo_conector_id",
                table: "data_cargador",
                column: "tipo_conector_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_electrolinera_compania_id",
                table: "data_electrolinera",
                column: "compania_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_observacion_electrolinera_id",
                table: "data_observacion",
                column: "electrolinera_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "data_auto");

            migrationBuilder.DropTable(
                name: "data_cargador");

            migrationBuilder.DropTable(
                name: "data_observacion");

            migrationBuilder.DropTable(
                name: "data_version");

            migrationBuilder.DropTable(
                name: "data_marcaauto");

            migrationBuilder.DropTable(
                name: "data_tipoconector");

            migrationBuilder.DropTable(
                name: "data_electrolinera");

            migrationBuilder.DropTable(
                name: "data_compania");
        }
    }
}

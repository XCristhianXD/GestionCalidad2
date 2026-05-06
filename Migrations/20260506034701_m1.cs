using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestionCalidad.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    id_Departamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.id_Departamento);
                });

            migrationBuilder.CreateTable(
                name: "InformeCalidad",
                columns: table => new
                {
                    Id_InformeCalidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Calificacion = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformeCalidad", x => x.Id_InformeCalidad);
                });

            migrationBuilder.CreateTable(
                name: "InformeQueja",
                columns: table => new
                {
                    id_InformeQueja = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformeQueja", x => x.id_InformeQueja);
                });

            migrationBuilder.CreateTable(
                name: "InformeCalidad_Departamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_Departamento = table.Column<int>(type: "integer", nullable: false),
                    id_InformeCalidad = table.Column<int>(type: "integer", nullable: false),
                    CodigoAtencion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformeCalidad_Departamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_InformeCalidad_Departamento_Departamento_id_Departamento",
                        column: x => x.id_Departamento,
                        principalTable: "Departamento",
                        principalColumn: "id_Departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformeCalidad_Departamento_InformeCalidad_id_InformeCalidad",
                        column: x => x.id_InformeCalidad,
                        principalTable: "InformeCalidad",
                        principalColumn: "Id_InformeCalidad",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformeQueja_Departamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_InformeQueja = table.Column<int>(type: "integer", nullable: false),
                    id_Departamento = table.Column<int>(type: "integer", nullable: false),
                    CodigoPaciente = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformeQueja_Departamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_InformeQueja_Departamento_Departamento_id_Departamento",
                        column: x => x.id_Departamento,
                        principalTable: "Departamento",
                        principalColumn: "id_Departamento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InformeQueja_Departamento_InformeQueja_id_InformeQueja",
                        column: x => x.id_InformeQueja,
                        principalTable: "InformeQueja",
                        principalColumn: "id_InformeQueja",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InformeCalidad_Departamento_id_Departamento",
                table: "InformeCalidad_Departamento",
                column: "id_Departamento");

            migrationBuilder.CreateIndex(
                name: "IX_InformeCalidad_Departamento_id_InformeCalidad",
                table: "InformeCalidad_Departamento",
                column: "id_InformeCalidad");

            migrationBuilder.CreateIndex(
                name: "IX_InformeQueja_Departamento_id_Departamento",
                table: "InformeQueja_Departamento",
                column: "id_Departamento");

            migrationBuilder.CreateIndex(
                name: "IX_InformeQueja_Departamento_id_InformeQueja",
                table: "InformeQueja_Departamento",
                column: "id_InformeQueja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InformeCalidad_Departamento");

            migrationBuilder.DropTable(
                name: "InformeQueja_Departamento");

            migrationBuilder.DropTable(
                name: "InformeCalidad");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "InformeQueja");
        }
    }
}

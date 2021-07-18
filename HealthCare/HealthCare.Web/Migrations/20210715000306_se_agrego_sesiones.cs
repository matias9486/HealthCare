using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCare.Web.Migrations
{
    public partial class se_agrego_sesiones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioCreacionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    PatologiaId = table.Column<int>(type: "int", nullable: false),
                    TratamientoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<double>(type: "float", nullable: false),
                    Presion = table.Column<double>(type: "float", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Medicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Automedicacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiagnosticoMedico = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sesiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sesiones_AspNetUsers_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sesiones_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sesiones_Patologias_PatologiaId",
                        column: x => x.PatologiaId,
                        principalTable: "Patologias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sesiones_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sesiones_Tratamientos_TratamientoId",
                        column: x => x.TratamientoId,
                        principalTable: "Tratamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_PacienteId",
                table: "Sesiones",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_PatologiaId",
                table: "Sesiones",
                column: "PatologiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_ProductoId",
                table: "Sesiones",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_TratamientoId",
                table: "Sesiones",
                column: "TratamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesiones_UsuarioCreacionId",
                table: "Sesiones",
                column: "UsuarioCreacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sesiones");
        }
    }
}

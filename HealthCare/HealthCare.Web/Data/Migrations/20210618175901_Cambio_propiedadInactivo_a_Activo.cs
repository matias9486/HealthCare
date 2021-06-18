using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCare.Web.Data.Migrations
{
    public partial class Cambio_propiedadInactivo_a_Activo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inactivo",
                table: "Productos",
                newName: "Activo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "Productos",
                newName: "Inactivo");
        }
    }
}

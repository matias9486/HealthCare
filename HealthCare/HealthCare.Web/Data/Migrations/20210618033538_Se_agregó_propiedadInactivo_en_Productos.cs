using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthCare.Web.Data.Migrations
{
    public partial class Se_agregó_propiedadInactivo_en_Productos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Inactivo",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inactivo",
                table: "Productos");
        }
    }
}

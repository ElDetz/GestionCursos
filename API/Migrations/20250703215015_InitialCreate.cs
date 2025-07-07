using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profesion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creditos = table.Column<int>(type: "int", nullable: false),
                    HorasSemanal = table.Column<int>(type: "int", nullable: false),
                    Ciclo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDocente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursos_Docentes_IdDocente",
                        column: x => x.IdDocente,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_IdDocente",
                table: "Cursos",
                column: "IdDocente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Docentes");
        }
    }
}

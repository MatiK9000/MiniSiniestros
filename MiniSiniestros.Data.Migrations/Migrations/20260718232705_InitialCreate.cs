using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSiniestros.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cuit = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrestadoresMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestadoresMedicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cuil = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Siniestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroSiniestro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    EmpleadorId = table.Column<int>(type: "int", nullable: false),
                    TrabajadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siniestros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siniestros_Empleadores_EmpleadorId",
                        column: x => x.EmpleadorId,
                        principalTable: "Empleadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siniestros_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistorialEstadosSiniestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiniestroId = table.Column<int>(type: "int", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "int", nullable: false),
                    EstadoNuevo = table.Column<int>(type: "int", nullable: false),
                    FechaCambio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEstadosSiniestros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEstadosSiniestros_Siniestros_SiniestroId",
                        column: x => x.SiniestroId,
                        principalTable: "Siniestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionesSrt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiniestroId = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exitosa = table.Column<bool>(type: "bit", nullable: false),
                    MensajeError = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionesSrt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificacionesSrt_Siniestros_SiniestroId",
                        column: x => x.SiniestroId,
                        principalTable: "Siniestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiniestrosPrestadores",
                columns: table => new
                {
                    SiniestroId = table.Column<int>(type: "int", nullable: false),
                    PrestadorMedicoId = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiniestrosPrestadores", x => new { x.SiniestroId, x.PrestadorMedicoId });
                    table.ForeignKey(
                        name: "FK_SiniestrosPrestadores_PrestadoresMedicos_PrestadorMedicoId",
                        column: x => x.PrestadorMedicoId,
                        principalTable: "PrestadoresMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiniestrosPrestadores_Siniestros_SiniestroId",
                        column: x => x.SiniestroId,
                        principalTable: "Siniestros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEstadosSiniestros_SiniestroId",
                table: "HistorialEstadosSiniestros",
                column: "SiniestroId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionesSrt_SiniestroId",
                table: "NotificacionesSrt",
                column: "SiniestroId");

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_EmpleadorId",
                table: "Siniestros",
                column: "EmpleadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_TrabajadorId",
                table: "Siniestros",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_SiniestrosPrestadores_PrestadorMedicoId",
                table: "SiniestrosPrestadores",
                column: "PrestadorMedicoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialEstadosSiniestros");

            migrationBuilder.DropTable(
                name: "NotificacionesSrt");

            migrationBuilder.DropTable(
                name: "SiniestrosPrestadores");

            migrationBuilder.DropTable(
                name: "PrestadoresMedicos");

            migrationBuilder.DropTable(
                name: "Siniestros");

            migrationBuilder.DropTable(
                name: "Empleadores");

            migrationBuilder.DropTable(
                name: "Trabajadores");
        }
    }
}

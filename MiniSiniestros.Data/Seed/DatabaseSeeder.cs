using Microsoft.EntityFrameworkCore;
using MiniSiniestros.Entities;

namespace MiniSiniestros.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Siniestros.AnyAsync())
        {
            return;
        }

        var empleadores = new List<Empleador>
        {
            new()
            {
                Cuit = "30123456789",
                RazonSocial = "Andina Construcciones S.A."
            },
            new()
            {
                Cuit = "30765432109",
                RazonSocial = "Logística del Sur S.R.L."
            }
        };

        var trabajadores = new List<Trabajador>
        {
            new()
            {
                Cuil = "20123456789",
                Nombre = "Juan",
                Apellido = "Pérez"
            },
            new()
            {
                Cuil = "27234567890",
                Nombre = "María",
                Apellido = "Gómez"
            },
            new()
            {
                Cuil = "20345678901",
                Nombre = "Carlos",
                Apellido = "Rodríguez"
            }
        };

        var prestadores = new List<PrestadorMedico>
        {
            new()
            {
                Nombre = "Clínica Central"
            },
            new()
            {
                Nombre = "Hospital del Trabajo"
            },
            new()
            {
                Nombre = "Centro Médico Andino"
            }
        };

        context.Empleadores.AddRange(empleadores);
        context.Trabajadores.AddRange(trabajadores);
        context.PrestadoresMedicos.AddRange(prestadores);

        await context.SaveChangesAsync();

        var siniestros = new List<Siniestro>
            {
                new()
                {
                    NumeroSiniestro = "SIN-2026-0001",
                    FechaAlta = DateTime.UtcNow.AddDays(-10),
                    Estado = EstadoSiniestro.Pendiente,
                    EmpleadorId = empleadores[0].Id,
                    TrabajadorId = trabajadores[0].Id
                },
                new()
                {
                    NumeroSiniestro = "SIN-2026-0002",
                    FechaAlta = DateTime.UtcNow.AddDays(-8),
                    Estado = EstadoSiniestro.EnProceso,
                    EmpleadorId = empleadores[0].Id,
                    TrabajadorId = trabajadores[1].Id
                },
                new()
                {
                    NumeroSiniestro = "SIN-2026-0003",
                    FechaAlta = DateTime.UtcNow.AddDays(-6),
                    Estado = EstadoSiniestro.Finalizado,
                    EmpleadorId = empleadores[1].Id,
                    TrabajadorId = trabajadores[2].Id
                },
                new()
                {
                    NumeroSiniestro = "SIN-2026-0004",
                    FechaAlta = DateTime.UtcNow.AddDays(-4),
                    Estado = EstadoSiniestro.Rechazado,
                    EmpleadorId = empleadores[1].Id,
                    TrabajadorId = trabajadores[0].Id
                },
                new()
                {
                    NumeroSiniestro = "SIN-2026-0005",
                    FechaAlta = DateTime.UtcNow.AddDays(-2),
                    Estado = EstadoSiniestro.Finalizado,
                    EmpleadorId = empleadores[0].Id,
                    TrabajadorId = trabajadores[2].Id
                }
            };

        context.Siniestros.AddRange(siniestros);

        await context.SaveChangesAsync();
    }
}
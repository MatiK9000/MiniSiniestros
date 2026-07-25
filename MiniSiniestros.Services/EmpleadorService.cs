using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniSiniestros.Data;
using MiniSiniestros.Entities;
using MiniSiniestros.Services.Exceptions;

namespace MiniSiniestros.Services;

public class EmpleadorService : IEmpleadorService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<EmpleadorService> logger;

    public EmpleadorService(ApplicationDbContext context, ILogger<EmpleadorService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<Empleador>> ObtenerTodosAsync()
    {
        return await context.Empleadores
            .AsNoTracking()
            .OrderBy(e => e.RazonSocial)
            .ToListAsync();
    }

    public async Task<Empleador?> ObtenerPorIdAsync(int id)
    {
        return await context.Empleadores
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Empleador> CrearAsync(Empleador empleador)
    {
        NormalizarDatos(empleador);

        bool existeCuit = await context.Empleadores
            .AnyAsync(e => e.Cuit == empleador.Cuit);

        if (existeCuit)
        {
            logger.LogWarning(
                "No se pudo crear el empleador porque el CUIT {Cuit} ya existe",
                empleador.Cuit);

            throw new ReglaNegocioException(
                "Ya existe un empleador con el CUIT ingresado.");
        }

        context.Empleadores.Add(empleador);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se creó el empleador {EmpleadorId} con CUIT {Cuit}",
            empleador.Id,
            empleador.Cuit);

        return empleador;
    }

    public async Task ModificarAsync(int id, Empleador empleador)
    {
        Empleador? empleadorExistente =
            await context.Empleadores.FindAsync(id);

        if (empleadorExistente is null)
        {
            logger.LogWarning(
                "No se encontró el empleador {EmpleadorId} para modificar",
                id);

            throw new ReglaNegocioException(
                "El empleador no existe.");
        }

        NormalizarDatos(empleador);

        bool existeOtroConMismoCuit = await context.Empleadores
            .AnyAsync(e =>
                e.Cuit == empleador.Cuit &&
                e.Id != id);

        if (existeOtroConMismoCuit)
        {
            logger.LogWarning(
                "No se pudo modificar el empleador {EmpleadorId} porque el CUIT {Cuit} pertenece a otro empleador",
                id,
                empleador.Cuit);

            throw new ReglaNegocioException(
                "Ya existe otro empleador con el CUIT ingresado.");
        }

        empleadorExistente.RazonSocial = empleador.RazonSocial;
        empleadorExistente.Cuit = empleador.Cuit;

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se modificó el empleador {EmpleadorId}",
            id);
    }

    public async Task EliminarAsync(int id)
    {
        Empleador? empleador =
            await context.Empleadores.FindAsync(id);

        if (empleador is null)
        {
            logger.LogWarning(
                "No se encontró el empleador {EmpleadorId} para eliminar",
                id);

            throw new ReglaNegocioException(
                "El empleador no existe.");
        }

        bool tieneSiniestros = await context.Siniestros
            .AnyAsync(s => s.EmpleadorId == id);

        if (tieneSiniestros)
        {
            logger.LogWarning(
                "No se puede eliminar el empleador {EmpleadorId} porque tiene siniestros asociados",
                id);

            throw new ReglaNegocioException(
                "No se puede eliminar el empleador porque tiene siniestros asociados.");
        }

        context.Empleadores.Remove(empleador);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se eliminó el empleador {EmpleadorId}",
            id);
    }

    private static void NormalizarDatos(Empleador empleador)
    {
        empleador.RazonSocial = empleador.RazonSocial.Trim();
        empleador.Cuit = empleador.Cuit.Trim();
    }
}
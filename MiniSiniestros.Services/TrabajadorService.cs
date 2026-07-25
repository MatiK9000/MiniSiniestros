using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniSiniestros.Data;
using MiniSiniestros.Entities;
using MiniSiniestros.Services.Exceptions;

namespace MiniSiniestros.Services;

public class TrabajadorService : ITrabajadorService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<TrabajadorService> logger;

    public TrabajadorService(ApplicationDbContext context, ILogger<TrabajadorService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<Trabajador>> ObtenerTodosAsync()
    {
        return await context.Trabajadores
            .AsNoTracking()
            .OrderBy(t => t.Apellido)
            .ThenBy(t => t.Nombre)
            .ToListAsync();
    }

    public async Task<Trabajador?> ObtenerPorIdAsync(int id)
    {
        return await context.Trabajadores
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Trabajador> CrearAsync(Trabajador trabajador)
    {
        NormalizarDatos(trabajador);

        bool existeCuil = await context.Trabajadores
            .AnyAsync(t => t.Cuil == trabajador.Cuil);

        if (existeCuil)
        {
            logger.LogWarning(
                "No se pudo crear el trabajador porque el CUIL {Cuil} ya existe",
                trabajador.Cuil);

            throw new ReglaNegocioException(
                "Ya existe un trabajador con el CUIL ingresado.");
        }

        context.Trabajadores.Add(trabajador);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se creó el trabajador {TrabajadorId} con CUIL {Cuil}",
            trabajador.Id,
            trabajador.Cuil);

        return trabajador;
    }

    public async Task ModificarAsync(int id, Trabajador trabajador)
    {
        Trabajador? trabajadorExistente =
            await context.Trabajadores.FindAsync(id);

        if (trabajadorExistente is null)
        {
            logger.LogWarning(
                "No se encontró el trabajador {TrabajadorId} para modificar",
                id);

            throw new ReglaNegocioException(
                "El trabajador no existe.");
        }

        NormalizarDatos(trabajador);

        bool existeOtroConMismoCuil = await context.Trabajadores
            .AnyAsync(t =>
                t.Cuil == trabajador.Cuil &&
                t.Id != id);

        if (existeOtroConMismoCuil)
        {
            logger.LogWarning(
                "No se pudo modificar el trabajador {TrabajadorId} porque el CUIL {Cuil} pertenece a otro trabajador",
                id,
                trabajador.Cuil);

            throw new ReglaNegocioException(
                "Ya existe otro trabajador con el CUIL ingresado.");
        }

        trabajadorExistente.Cuil = trabajador.Cuil;
        trabajadorExistente.Nombre = trabajador.Nombre;
        trabajadorExistente.Apellido = trabajador.Apellido;

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se modificó el trabajador {TrabajadorId}",
            id);
    }

    public async Task EliminarAsync(int id)
    {
        Trabajador? trabajador =
            await context.Trabajadores.FindAsync(id);

        if (trabajador is null)
        {
            logger.LogWarning(
                "No se encontró el trabajador {TrabajadorId} para eliminar",
                id);

            throw new ReglaNegocioException(
                "El trabajador no existe.");
        }

        bool tieneSiniestros = await context.Siniestros
            .AnyAsync(s => s.TrabajadorId == id);

        if (tieneSiniestros)
        {
            logger.LogWarning(
                "No se puede eliminar el trabajador {TrabajadorId} porque tiene siniestros asociados",
                id);

            throw new ReglaNegocioException(
                "No se puede eliminar el trabajador porque tiene siniestros asociados.");
        }

        context.Trabajadores.Remove(trabajador);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se eliminó el trabajador {TrabajadorId}",
            id);
    }

    private static void NormalizarDatos(Trabajador trabajador)
    {
        trabajador.Cuil = trabajador.Cuil.Trim();
        trabajador.Nombre = trabajador.Nombre.Trim();
        trabajador.Apellido = trabajador.Apellido.Trim();
    }
}
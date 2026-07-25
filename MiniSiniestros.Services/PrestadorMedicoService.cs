using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniSiniestros.Data;
using MiniSiniestros.Entities;
using MiniSiniestros.Services.Exceptions;

namespace MiniSiniestros.Services;

public class PrestadorMedicoService : IPrestadorMedicoService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<PrestadorMedicoService> logger;

    public PrestadorMedicoService(ApplicationDbContext context, ILogger<PrestadorMedicoService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<PrestadorMedico>> ObtenerTodosAsync()
    {
        return await context.PrestadoresMedicos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<PrestadorMedico?> ObtenerPorIdAsync(int id)
    {
        return await context.PrestadoresMedicos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PrestadorMedico> CrearAsync(
        PrestadorMedico prestadorMedico)
    {
        NormalizarDatos(prestadorMedico);

        var nombreDuplicado = await context.PrestadoresMedicos
            .AnyAsync(p => p.Nombre == prestadorMedico.Nombre);

        if (nombreDuplicado)
        {
            throw new ReglaNegocioException(
                "Ya existe un prestador médico con ese nombre.");
        }

        context.PrestadoresMedicos.Add(prestadorMedico);
        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se creó el prestador médico {PrestadorMedicoId} con nombre {Nombre}",
            prestadorMedico.Id,
            prestadorMedico.Nombre);

        return prestadorMedico;
    }

    public async Task ModificarAsync(
        int id,
        PrestadorMedico prestadorMedico)
    {
        var prestadorExistente = await context.PrestadoresMedicos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prestadorExistente is null)
        {
            throw new ReglaNegocioException(
                "El prestador médico no existe.");
        }

        NormalizarDatos(prestadorMedico);

        var nombreDuplicado = await context.PrestadoresMedicos
            .AnyAsync(p =>
                p.Nombre == prestadorMedico.Nombre &&
                p.Id != id);

        if (nombreDuplicado)
        {
            throw new ReglaNegocioException(
                "Ya existe otro prestador médico con ese nombre.");
        }

        prestadorExistente.Nombre = prestadorMedico.Nombre;

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se modificó el prestador médico {PrestadorMedicoId}",
            id);
    }

    public async Task EliminarAsync(int id)
    {
        var prestadorMedico = await context.PrestadoresMedicos
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prestadorMedico is null)
        {
            throw new ReglaNegocioException(
                "El prestador médico no existe.");
        }

        var tieneSiniestrosAsignados = await context.SiniestrosPrestadores
            .AnyAsync(sp => sp.PrestadorMedicoId == id);

        if (tieneSiniestrosAsignados)
        {
            throw new ReglaNegocioException(
                "No se puede eliminar el prestador médico porque tiene siniestros asignados.");
        }

        context.PrestadoresMedicos.Remove(prestadorMedico);
        await context.SaveChangesAsync();

        logger.LogInformation(
            "Se eliminó el prestador médico {PrestadorMedicoId}",
            id);
    }

    private static void NormalizarDatos(
        PrestadorMedico prestadorMedico)
    {
        prestadorMedico.Nombre = prestadorMedico.Nombre.Trim();
    }
}
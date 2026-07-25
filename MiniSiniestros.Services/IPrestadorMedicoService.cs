using MiniSiniestros.Entities;

namespace MiniSiniestros.Services;

public interface IPrestadorMedicoService
{
    Task<IEnumerable<PrestadorMedico>> ObtenerTodosAsync();

    Task<PrestadorMedico?> ObtenerPorIdAsync(int id);

    Task<PrestadorMedico> CrearAsync(PrestadorMedico prestadorMedico);

    Task ModificarAsync(int id, PrestadorMedico prestadorMedico);

    Task EliminarAsync(int id);
}
using MiniSiniestros.Entities;

namespace MiniSiniestros.Services;

public interface ITrabajadorService
{
    Task<IEnumerable<Trabajador>> ObtenerTodosAsync();

    Task<Trabajador?> ObtenerPorIdAsync(int id);

    Task<Trabajador> CrearAsync(Trabajador trabajador);

    Task ModificarAsync(int id, Trabajador trabajador);

    Task EliminarAsync(int id);
}
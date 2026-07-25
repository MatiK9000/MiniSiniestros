using MiniSiniestros.Entities;

namespace MiniSiniestros.Services;

public interface IEmpleadorService
{
    Task<IEnumerable<Empleador>> ObtenerTodosAsync();

    Task<Empleador?> ObtenerPorIdAsync(int id);

    Task<Empleador> CrearAsync(Empleador empleador);

    Task ModificarAsync(int id, Empleador empleador);

    Task EliminarAsync(int id);
}
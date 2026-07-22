using MiniSiniestros.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Services
{
    public interface ISiniestroService
    {
        Task<List<Siniestro>> ObtenerTodosAsync();

        Task<Siniestro?> ObtenerPorIdAsync(int id);

        Task<Siniestro> CrearAsync(Siniestro siniestro);

        Task<bool> CambiarEstadoAsync(
            int siniestroId,
            EstadoSiniestro nuevoEstado);

        Task<bool> AsignarPrestadorAsync(
            int siniestroId,
            int prestadorMedicoId);
    }
}

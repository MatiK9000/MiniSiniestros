using MiniSiniestros.Data;
using MiniSiniestros.Entities;
using Microsoft.EntityFrameworkCore;


namespace MiniSiniestros.Services
{
    public class SiniestroService : ISiniestroService
    {
        private readonly ApplicationDbContext context;

        public SiniestroService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AsignarPrestadorAsync(int siniestroId, int prestadorMedicoId)
        {
            bool existeSiniestro = await context.Siniestros
        .AnyAsync(s => s.Id == siniestroId);

            if (!existeSiniestro)
            {
                return false;
            }

            bool existePrestador = await context.PrestadoresMedicos
                .AnyAsync(p => p.Id == prestadorMedicoId);

            if (!existePrestador)
            {
                return false;
            }

            bool yaEstaAsignado = await context.SiniestrosPrestadores
                .AnyAsync(sp =>
                    sp.SiniestroId == siniestroId &&
                    sp.PrestadorMedicoId == prestadorMedicoId);

            if (yaEstaAsignado)
            {
                return false;
            }

            SiniestroPrestador asignacion = new SiniestroPrestador
            {
                SiniestroId = siniestroId,
                PrestadorMedicoId = prestadorMedicoId,
                FechaAsignacion = DateTime.Now
            };

            context.SiniestrosPrestadores.Add(asignacion);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int siniestroId, EstadoSiniestro nuevoEstado)
        {
            Siniestro? siniestro = await context.Siniestros
        .FirstOrDefaultAsync(s => s.Id == siniestroId);

            if (siniestro is null)
            {
                return false;
            }

            EstadoSiniestro estadoAnterior = siniestro.Estado;

            siniestro.Estado = nuevoEstado;

            HistorialEstadoSiniestro historial = new HistorialEstadoSiniestro
            {
                SiniestroId = siniestro.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = nuevoEstado,
                FechaCambio = DateTime.Now
            };

            context.HistorialEstadosSiniestros.Add(historial);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<Siniestro> CrearAsync(Siniestro siniestro)
        {
            bool empleadorExiste = await context.Empleadores
        .AnyAsync(e => e.Id == siniestro.EmpleadorId);

            if (!empleadorExiste)
            {
                throw new ArgumentException("El empleador indicado no existe.");
            }

            bool trabajadorExiste = await context.Trabajadores
                .AnyAsync(t => t.Id == siniestro.TrabajadorId);

            if (!trabajadorExiste)
            {
                throw new ArgumentException("El trabajador indicado no existe.");
            }

            siniestro.FechaAlta = DateTime.Now;
            siniestro.Estado = EstadoSiniestro.Pendiente;

            context.Siniestros.Add(siniestro);

            await context.SaveChangesAsync();

            return siniestro;
        }

        public async Task<Siniestro?> ObtenerPorIdAsync(int id)
        {
            return await context.Siniestros
                .AsNoTracking()
                .Include(s => s.Empleador)
                .Include(s => s.Trabajador)
                .Include(s => s.Prestadores)
                    .ThenInclude(sp => sp.PrestadorMedico)
                .Include(s => s.HistorialEstados)
                .Include(s => s.NotificacionesSrt)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Siniestro>> ObtenerTodosAsync()
        {
            return await context.Siniestros
                .AsNoTracking()//Le indica a Entity Framework que estos objetos son solo para consulta. Como no los vamos a modificar en este método, EF no necesita seguir sus cambios.
                .Include(s => s.Empleador)//Trae también los datos relacionados del empleador y del trabajador.
                .Include(s => s.Trabajador)
                .ToListAsync();
        }
    }
}

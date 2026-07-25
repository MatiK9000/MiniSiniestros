using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniSiniestros.Data;
using MiniSiniestros.Entities;
using MiniSiniestros.Services.Exceptions;


namespace MiniSiniestros.Services
{
    public class SiniestroService : ISiniestroService
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<SiniestroService> logger;

        public SiniestroService(ApplicationDbContext context, ILogger<SiniestroService> logger)
        {
            this.context = context;
            this.logger = logger;
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
                throw new ReglaNegocioException("El prestador ya está asignado al siniestro.");
            }

            SiniestroPrestador asignacion = new SiniestroPrestador
            {
                SiniestroId = siniestroId,
                PrestadorMedicoId = prestadorMedicoId,
                FechaAsignacion = DateTime.Now
            };

            context.SiniestrosPrestadores.Add(asignacion);

            await context.SaveChangesAsync();

            logger.LogInformation(
                    "Se asignó el prestador {PrestadorId} al siniestro {SiniestroId}.",
                    prestadorMedicoId,
                    siniestroId);

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

            logger.LogInformation(
                    "El siniestro {SiniestroId} cambió del estado {EstadoAnterior} al estado {EstadoNuevo}.",
                    siniestro.Id,
                    estadoAnterior,
                    nuevoEstado);

            return true;
        }

        public async Task<Siniestro> CrearAsync(Siniestro siniestro)
        {
            bool empleadorExiste = await context.Empleadores
        .AnyAsync(e => e.Id == siniestro.EmpleadorId);

            if (!empleadorExiste)
            {
                logger.LogWarning(
                    "No se pudo crear el siniestro porque el empleador {EmpleadorId} no existe.",
                    siniestro.EmpleadorId);

                throw new ReglaNegocioException("El empleador no existe.");
            }

            bool trabajadorExiste = await context.Trabajadores
                .AnyAsync(t => t.Id == siniestro.TrabajadorId);

            if (!trabajadorExiste)
            {
                throw new ReglaNegocioException("El trabajador no existe.");
            }

            siniestro.FechaAlta = DateTime.Now;
            siniestro.Estado = EstadoSiniestro.Pendiente;

            context.Siniestros.Add(siniestro);

            await context.SaveChangesAsync();

            logger.LogInformation(
                    "Se creó el siniestro {NumeroSiniestro} con Id {SiniestroId}.",
                    siniestro.NumeroSiniestro,
                    siniestro.Id);

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

        public async Task<(IEnumerable<Siniestro> Elementos, int TotalRegistros)> ObtenerTodosAsync(int pagina, int tamanioPagina, EstadoSiniestro? estado, string? numero, int? empleadorId)
        {
            IQueryable<Siniestro> consulta = context.Siniestros
             .AsNoTracking()
             .Include(s => s.Empleador)
             .Include(s => s.Trabajador);

            if (estado.HasValue)
            {
                consulta = consulta.Where(s => s.Estado == estado.Value);
            }

            if (!string.IsNullOrWhiteSpace(numero))
            {
                consulta = consulta.Where(
                    s => s.NumeroSiniestro.Contains(numero));
            }

            if (empleadorId.HasValue)
            {
                consulta = consulta.Where(
                    s => s.EmpleadorId == empleadorId.Value);
            }

            int totalRegistros = await consulta.CountAsync();

            List<Siniestro> elementos = await consulta
                .OrderByDescending(s => s.FechaAlta)
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();

            return (elementos, totalRegistros);
        }
    }
}

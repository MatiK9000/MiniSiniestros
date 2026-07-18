using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Entities
{
    public class Siniestro
    {
        public class Siniestro
        {
            public int Id { get; set; }

            public string NumeroSiniestro { get; set; }

            public DateTime FechaAlta { get; set; }

            public EstadoSiniestro Estado { get; set; }

            // FK Empleador
            public int EmpleadorId { get; set; }
            public Empleador Empleador { get; set; }

            // FK Trabajador
            public int TrabajadorId { get; set; }
            public Trabajador Trabajador { get; set; }

            // Prestadores asignados
            public ICollection<SiniestroPrestador> Prestadores { get; set; } = [];

            // Historial de cambios de estado
            public ICollection<HistorialEstadoSiniestro> HistorialEstados { get; set; } = [];

            // Solo si implementamos la integración opcional
            public ICollection<NotificacionSrt> NotificacionesSrt { get; set; } = [];
        }
    }
}

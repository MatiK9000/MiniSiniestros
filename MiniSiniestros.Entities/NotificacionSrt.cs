using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Entities
{
    public class NotificacionSrt
    {
        public int Id { get; set; }

        public int SiniestroId { get; set; }

        public required Siniestro Siniestro { get; set; }

        public DateTime FechaEnvio { get; set; }

        public bool Exitosa { get; set; }

        public string? MensajeError { get; set; }
    }
}

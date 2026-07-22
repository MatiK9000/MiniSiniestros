using MiniSiniestros.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Dto
{
    public class SiniestroDto
    {
        public int Id { get; set; }

        public string NumeroSiniestro { get; set; } = string.Empty;

        public DateTime FechaAlta { get; set; }

        public EstadoSiniestro Estado { get; set; }

        public int EmpleadorId { get; set; }

        public string EmpleadorRazonSocial { get; set; } = string.Empty;

        public int TrabajadorId { get; set; }

        public string TrabajadorNombreCompleto { get; set; } = string.Empty;
    }
}

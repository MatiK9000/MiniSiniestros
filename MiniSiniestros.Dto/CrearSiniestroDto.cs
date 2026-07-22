using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Dto
{
    public class CrearSiniestroDto
    {
        public required string NumeroSiniestro { get; set; }

        public int EmpleadorId { get; set; }

        public int TrabajadorId { get; set; }
    }
}

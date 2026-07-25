using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Dto
{
    public class CrearSiniestroDto
    {
        [Required]
        public string NumeroSiniestro { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int EmpleadorId { get; set; }

        [Range(1, int.MaxValue)]
        public int TrabajadorId { get; set; }
    }
}

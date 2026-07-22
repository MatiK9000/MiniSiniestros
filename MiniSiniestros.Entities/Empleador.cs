using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Entities
{
    public class Empleador
    {
        public int Id { get; set; }

        public required string Cuit { get; set; }

        public required string RazonSocial { get; set; }

        public ICollection<Siniestro> Siniestros { get; set; } = [];
    }
}

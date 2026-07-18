using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Entities
{
    public class PrestadorMedico
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public ICollection<SiniestroPrestador> Siniestros { get; set; } = [];
    }
}

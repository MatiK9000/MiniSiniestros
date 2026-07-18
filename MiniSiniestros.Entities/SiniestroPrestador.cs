using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSiniestros.Entities
{
    public class SiniestroPrestador
    {
        public int SiniestroId { get; set; }
        public Siniestro Siniestro { get; set; }

        public int PrestadorMedicoId { get; set; }
        public PrestadorMedico PrestadorMedico { get; set; }

        public DateTime FechaAsignacion { get; set; }
    }
}

using MiniSiniestros.Entities;

namespace MiniSiniestros.ViewModels;

public class FiltroSiniestrosViewModel
{
    public EstadoSiniestro? Estado { get; set; }

    public string? Numero { get; set; }

    public int? EmpleadorId { get; set; }

    public int Pagina { get; set; } = 1;

    public int TamanioPagina { get; set; } = 10;
}
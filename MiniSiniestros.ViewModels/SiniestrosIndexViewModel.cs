using MiniSiniestros.Entities;

namespace MiniSiniestros.ViewModels;

public class SiniestrosIndexViewModel
{
    public FiltroSiniestrosViewModel Filtros { get; set; } = new();

    public IEnumerable<SiniestroListadoItemViewModel> Siniestros { get; set; }
        = [];

    public IEnumerable<ComboItemViewModel> Empleadores { get; set; }
        = [];

    public int TotalRegistros { get; set; }

    public int TotalPaginas { get; set; }

    public IEnumerable<EstadoSiniestro> Estados =>
        Enum.GetValues<EstadoSiniestro>();
}
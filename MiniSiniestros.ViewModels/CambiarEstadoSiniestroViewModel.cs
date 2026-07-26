using MiniSiniestros.Entities;
using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.ViewModels;

public class CambiarEstadoSiniestroViewModel
{
    public int SiniestroId { get; set; }

    public string Numero { get; set; } = string.Empty;

    public string EstadoActual { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Nuevo estado")]
    public EstadoSiniestro? NuevoEstado { get; set; }

    public IEnumerable<EstadoSiniestro> Estados =>
        Enum.GetValues<EstadoSiniestro>();
}
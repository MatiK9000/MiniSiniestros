using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.ViewModels;

public class CrearSiniestroViewModel
{
    [Required]
    [Display(Name = "Número")]
    public string Numero { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Empleador")]
    public int? EmpleadorId { get; set; }

    [Required]
    [Display(Name = "Trabajador")]
    public int? TrabajadorId { get; set; }

    public IEnumerable<ComboItemViewModel> Empleadores { get; set; } = [];

    public IEnumerable<ComboItemViewModel> Trabajadores { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.ViewModels;

public class TrabajadorFormularioViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "CUIL")]
    public string Cuil { get; set; } = string.Empty;

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;
}
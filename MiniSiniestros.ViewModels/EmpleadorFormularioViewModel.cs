using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.ViewModels;

public class EmpleadorFormularioViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "CUIT")]
    public string Cuit { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Razón social")]
    public string RazonSocial { get; set; } = string.Empty;
}
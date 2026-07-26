using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.ViewModels;

public class PrestadorMedicoFormularioViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;
}
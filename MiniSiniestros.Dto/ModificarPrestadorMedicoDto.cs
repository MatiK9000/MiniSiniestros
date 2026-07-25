using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.Dto;

public class ModificarPrestadorMedicoDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.Dto;

public class ModificarEmpleadorDto
{
    [Required(ErrorMessage = "La razón social es obligatoria.")]
    [StringLength(
        150,
        ErrorMessage = "La razón social no puede superar los 150 caracteres.")]
    public string RazonSocial { get; set; } = string.Empty;

    [Required(ErrorMessage = "El CUIT es obligatorio.")]
    [StringLength(
        11,
        MinimumLength = 11,
        ErrorMessage = "El CUIT debe tener 11 caracteres.")]
    public string Cuit { get; set; } = string.Empty;
}
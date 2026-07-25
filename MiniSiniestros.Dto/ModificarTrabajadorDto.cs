using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.Dto;

public class ModificarTrabajadorDto
{
    [Required(ErrorMessage = "El CUIL es obligatorio.")]
    [StringLength(
        11,
        MinimumLength = 11,
        ErrorMessage = "El CUIL debe tener 11 caracteres.")]
    public string Cuil { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(
        100,
        ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(
        100,
        ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;
}
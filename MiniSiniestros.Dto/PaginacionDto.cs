using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.Dto;

public class PaginacionDto
{
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "La página debe ser mayor o igual a 1.")]
    public int Pagina { get; set; } = 1;

    [Range(
        1,
        100,
        ErrorMessage = "El tamaño de página debe estar entre 1 y 100.")]
    public int TamanioPagina { get; set; } = 10;
}
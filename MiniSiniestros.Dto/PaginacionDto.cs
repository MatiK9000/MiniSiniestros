using System.ComponentModel.DataAnnotations;

namespace MiniSiniestros.Dto;

public class PaginacionDto
{
    [Range(1, int.MaxValue)]
    public int Pagina { get; set; } = 1;

    [Range(1, 100)]
    public int TamanioPagina { get; set; } = 10;
}
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;
using System.ComponentModel.DataAnnotations;

public class FiltroSiniestrosDto : PaginacionDto
{
    public EstadoSiniestro? Estado { get; set; }

    public string? Numero { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El empleador debe ser mayor a 0.")]
    public int? EmpleadorId { get; set; }
}
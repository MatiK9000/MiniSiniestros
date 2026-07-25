namespace MiniSiniestros.Dto;

public class RespuestaPaginadaDto<T>
{
    public IEnumerable<T> Elementos { get; set; } = [];

    public int PaginaActual { get; set; }

    public int TamanioPagina { get; set; }

    public int TotalRegistros { get; set; }

    public int TotalPaginas { get; set; }
}
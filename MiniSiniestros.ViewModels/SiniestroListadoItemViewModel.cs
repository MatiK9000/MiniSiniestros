namespace MiniSiniestros.ViewModels;

public class SiniestroListadoItemViewModel
{
    public int Id { get; set; }

    public string Numero { get; set; } = string.Empty;

    public DateTime FechaAlta { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string Empleador { get; set; } = string.Empty;

    public string Trabajador { get; set; } = string.Empty;
}
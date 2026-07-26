namespace MiniSiniestros.ViewModels;

public class TrabajadorListadoItemViewModel
{
    public int Id { get; set; }

    public string Cuil { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
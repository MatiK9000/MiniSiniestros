namespace MiniSiniestros.ViewModels;

public class TrabajadorDetalleViewModel
{
    public int Id { get; set; }

    public string Cuil { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
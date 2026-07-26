using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;
using MiniSiniestros.ViewModels;

namespace MiniSiniestros.Web.Controllers;

public class SiniestrosController : Controller
{
    private readonly ISiniestroService siniestroService;
    private readonly IEmpleadorService empleadorService;
    private readonly ITrabajadorService trabajadorService;

    public SiniestrosController(
        ISiniestroService siniestroService,
        IEmpleadorService empleadorService,
        ITrabajadorService trabajadorService)
    {
        this.siniestroService = siniestroService;
        this.empleadorService = empleadorService;
        this.trabajadorService = trabajadorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        FiltroSiniestrosViewModel filtros)
    {
        if (filtros.Pagina < 1)
        {
            filtros.Pagina = 1;
        }

        if (filtros.TamanioPagina < 1)
        {
            filtros.TamanioPagina = 10;
        }

        var resultado = await siniestroService.ObtenerTodosAsync(
            filtros.Pagina,
            filtros.TamanioPagina,
            filtros.Estado,
            filtros.Numero,
            filtros.EmpleadorId);

        var empleadores = await empleadorService.ObtenerTodosAsync();

        var modelo = new SiniestrosIndexViewModel
        {
            Filtros = filtros,

            TotalRegistros = resultado.TotalRegistros,

            TotalPaginas = (int)Math.Ceiling(
                resultado.TotalRegistros /
                (double)filtros.TamanioPagina),

            Empleadores = empleadores
                .Select(e => new ComboItemViewModel
                {
                    Id = e.Id,
                    Nombre = e.RazonSocial
                })
                .ToList(),

            Siniestros = resultado.Elementos
                .Select(s => new SiniestroListadoItemViewModel
                {
                    Id = s.Id,
                    Numero = s.NumeroSiniestro,
                    FechaAlta = s.FechaAlta,
                    Estado = s.Estado.ToString(),
                    Empleador = s.Empleador.RazonSocial,
                    Trabajador =
                        $"{s.Trabajador.Apellido}, {s.Trabajador.Nombre}"
                })
                .ToList()
        };

        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var modelo = new CrearSiniestroViewModel();

        await CargarCombosAsync(modelo);

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CrearSiniestroViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarCombosAsync(modelo);

            return View(modelo);
        }

        var siniestro = new Siniestro
        {
            NumeroSiniestro = modelo.Numero,
            EmpleadorId = modelo.EmpleadorId!.Value,
            TrabajadorId = modelo.TrabajadorId!.Value
        };

        await siniestroService.CrearAsync(siniestro);

        TempData["MensajeExito"] = "El siniestro se creó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCombosAsync(
        CrearSiniestroViewModel modelo)
    {
        var empleadores =
            await empleadorService.ObtenerTodosAsync();

        var trabajadores =
            await trabajadorService.ObtenerTodosAsync();

        modelo.Empleadores = empleadores
            .Select(e => new ComboItemViewModel
            {
                Id = e.Id,
                Nombre = e.RazonSocial
            })
            .ToList();

        modelo.Trabajadores = trabajadores
            .Select(t => new ComboItemViewModel
            {
                Id = t.Id,
                Nombre = $"{t.Apellido}, {t.Nombre}"
            })
            .ToList();
    }
}
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;
using MiniSiniestros.ViewModels;

namespace MiniSiniestros.Web.Controllers;

public class TrabajadoresController : Controller
{
    private readonly ITrabajadorService trabajadorService;

    public TrabajadoresController(ITrabajadorService trabajadorService)
    {
        this.trabajadorService = trabajadorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var trabajadores = await trabajadorService.ObtenerTodosAsync();

        var modelo = trabajadores
            .Select(t => new TrabajadorListadoItemViewModel
            {
                Id = t.Id,
                Cuil = t.Cuil,
                Nombre = t.Nombre,
                Apellido = t.Apellido
            })
            .ToList();

        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var trabajador = await trabajadorService.ObtenerPorIdAsync(id);

        if (trabajador is null)
        {
            return NotFound();
        }

        var modelo = new TrabajadorDetalleViewModel
        {
            Id = trabajador.Id,
            Cuil = trabajador.Cuil,
            Nombre = trabajador.Nombre,
            Apellido = trabajador.Apellido
        };

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new TrabajadorFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TrabajadorFormularioViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var trabajador = new Trabajador
        {
            Cuil = modelo.Cuil,
            Nombre = modelo.Nombre,
            Apellido = modelo.Apellido
        };

        await trabajadorService.CrearAsync(trabajador);

        TempData["MensajeExito"] =
            "El trabajador se creó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var trabajador = await trabajadorService.ObtenerPorIdAsync(id);

        if (trabajador is null)
        {
            return NotFound();
        }

        var modelo = new TrabajadorFormularioViewModel
        {
            Id = trabajador.Id,
            Cuil = trabajador.Cuil,
            Nombre = trabajador.Nombre,
            Apellido = trabajador.Apellido
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        TrabajadorFormularioViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var trabajador = new Trabajador
        {
            Id = modelo.Id,
            Cuil = modelo.Cuil,
            Nombre = modelo.Nombre,
            Apellido = modelo.Apellido
        };

        await trabajadorService.ModificarAsync(id, trabajador);

        TempData["MensajeExito"] =
            "El trabajador se modificó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var trabajador = await trabajadorService.ObtenerPorIdAsync(id);

        if (trabajador is null)
        {
            return NotFound();
        }

        var modelo = new TrabajadorDetalleViewModel
        {
            Id = trabajador.Id,
            Cuil = trabajador.Cuil,
            Nombre = trabajador.Nombre,
            Apellido = trabajador.Apellido
        };

        return View(modelo);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarDelete(int id)
    {
        await trabajadorService.EliminarAsync(id);

        TempData["MensajeExito"] =
            "El trabajador se eliminó correctamente.";

        return RedirectToAction(nameof(Index));
    }
}
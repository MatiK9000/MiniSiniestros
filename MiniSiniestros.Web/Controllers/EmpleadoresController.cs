using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;
using MiniSiniestros.ViewModels;

namespace MiniSiniestros.Web.Controllers;

public class EmpleadoresController : Controller
{
    private readonly IEmpleadorService empleadorService;

    public EmpleadoresController(IEmpleadorService empleadorService)
    {
        this.empleadorService = empleadorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var empleadores = await empleadorService.ObtenerTodosAsync();

        var modelo = empleadores
            .Select(e => new EmpleadorListadoItemViewModel
            {
                Id = e.Id,
                Cuit = e.Cuit,
                RazonSocial = e.RazonSocial
            })
            .ToList();

        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var empleador = await empleadorService.ObtenerPorIdAsync(id);

        if (empleador is null)
        {
            return NotFound();
        }

        var modelo = new EmpleadorDetalleViewModel
        {
            Id = empleador.Id,
            Cuit = empleador.Cuit,
            RazonSocial = empleador.RazonSocial
        };

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new EmpleadorFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmpleadorFormularioViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var empleador = new Empleador
        {
            Cuit = modelo.Cuit,
            RazonSocial = modelo.RazonSocial
        };

        await empleadorService.CrearAsync(empleador);

        TempData["MensajeExito"] =
            "El empleador se creó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var empleador = await empleadorService.ObtenerPorIdAsync(id);

        if (empleador is null)
        {
            return NotFound();
        }

        var modelo = new EmpleadorFormularioViewModel
        {
            Id = empleador.Id,
            Cuit = empleador.Cuit,
            RazonSocial = empleador.RazonSocial
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    int id,
    EmpleadorFormularioViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var empleador = new Empleador
        {
            Id = modelo.Id,
            Cuit = modelo.Cuit,
            RazonSocial = modelo.RazonSocial
        };

        await empleadorService.ModificarAsync(id, empleador);

        TempData["MensajeExito"] =
            "El empleador se modificó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var empleador = await empleadorService.ObtenerPorIdAsync(id);

        if (empleador is null)
        {
            return NotFound();
        }

        var modelo = new EmpleadorDetalleViewModel
        {
            Id = empleador.Id,
            Cuit = empleador.Cuit,
            RazonSocial = empleador.RazonSocial
        };

        return View(modelo);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarDelete(int id)
    {
        await empleadorService.EliminarAsync(id);

        TempData["MensajeExito"] =
            "El empleador se eliminó correctamente.";

        return RedirectToAction(nameof(Index));
    }
}
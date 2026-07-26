using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;
using MiniSiniestros.ViewModels;

namespace MiniSiniestros.Web.Controllers;

public class PrestadoresMedicosController : Controller
{
    private readonly IPrestadorMedicoService prestadorMedicoService;

    public PrestadoresMedicosController(
        IPrestadorMedicoService prestadorMedicoService)
    {
        this.prestadorMedicoService = prestadorMedicoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var prestadores =
            await prestadorMedicoService.ObtenerTodosAsync();

        var modelo = prestadores
            .Select(p => new PrestadorMedicoListadoItemViewModel
            {
                Id = p.Id,
                Nombre = p.Nombre
            })
            .ToList();

        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var prestador =
            await prestadorMedicoService.ObtenerPorIdAsync(id);

        if (prestador is null)
        {
            return NotFound();
        }

        var modelo = new PrestadorMedicoDetalleViewModel
        {
            Id = prestador.Id,
            Nombre = prestador.Nombre
        };

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new PrestadorMedicoFormularioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        PrestadorMedicoFormularioViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var prestador = new PrestadorMedico
        {
            Nombre = modelo.Nombre
        };

        await prestadorMedicoService.CrearAsync(prestador);

        TempData["MensajeExito"] =
            "El prestador médico se creó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var prestador =
            await prestadorMedicoService.ObtenerPorIdAsync(id);

        if (prestador is null)
        {
            return NotFound();
        }

        var modelo = new PrestadorMedicoFormularioViewModel
        {
            Id = prestador.Id,
            Nombre = prestador.Nombre
        };

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        PrestadorMedicoFormularioViewModel modelo)
    {
        if (id != modelo.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var prestador = new PrestadorMedico
        {
            Id = modelo.Id,
            Nombre = modelo.Nombre
        };

        await prestadorMedicoService.ModificarAsync(id, prestador);

        TempData["MensajeExito"] =
            "El prestador médico se modificó correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var prestador =
            await prestadorMedicoService.ObtenerPorIdAsync(id);

        if (prestador is null)
        {
            return NotFound();
        }

        var modelo = new PrestadorMedicoDetalleViewModel
        {
            Id = prestador.Id,
            Nombre = prestador.Nombre
        };

        return View(modelo);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarDelete(int id)
    {
        await prestadorMedicoService.EliminarAsync(id);

        TempData["MensajeExito"] =
            "El prestador médico se eliminó correctamente.";

        return RedirectToAction(nameof(Index));
    }
}
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.ViewModels;

namespace MiniSiniestros.Web.Controllers;

public class ErrorController : Controller
{
    [HttpGet]
    public IActionResult Negocio(string mensaje)
    {
        var modelo = new ErrorNegocioViewModel
        {
            Mensaje = mensaje,
            UrlAnterior = Request.Headers.Referer.ToString()
        };

        return View(modelo);
    }

    [HttpGet]
    public IActionResult General()
    {
        return View();
    }
}

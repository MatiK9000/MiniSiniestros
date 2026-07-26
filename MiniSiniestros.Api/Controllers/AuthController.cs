using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Dto;
using MiniSiniestros.Services;

namespace MiniSiniestros.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(
        LoginRequestDTO request)
    {
        var respuesta = await authService.LoginAsync(request);

        if (respuesta is null)
        {
            return Unauthorized(new
            {
                mensaje = "Usuario o contraseña incorrectos."
            });
        }

        return Ok(respuesta);
    }
}
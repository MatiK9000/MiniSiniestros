using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniSiniestros.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSiniestros.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration configuration;

    public AuthService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
    {
        if (request.Usuario != "operador" ||
            request.Password != "operador123")
        {
            return Task.FromResult<LoginResponseDTO?>(null);
        }

        var clave = configuration["Jwt:Clave"]
            ?? throw new InvalidOperationException(
                "No se configuró la clave JWT.");

        var emisor = configuration["Jwt:Emisor"]
            ?? throw new InvalidOperationException(
                "No se configuró el emisor JWT.");

        var audiencia = configuration["Jwt:Audiencia"]
            ?? throw new InvalidOperationException(
                "No se configuró la audiencia JWT.");

        var expiracionMinutos =
            configuration.GetValue<int>("Jwt:ExpiracionMinutos");

        var fechaExpiracion =
            DateTime.UtcNow.AddMinutes(expiracionMinutos);

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                request.Usuario),

            new(
                ClaimTypes.Name,
                request.Usuario),

            new(
                ClaimTypes.Role,
                "Operador"),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var claveSeguridad = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(clave));

        var credenciales = new SigningCredentials(
            claveSeguridad,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: emisor,
            audience: audiencia,
            claims: claims,
            expires: fechaExpiracion,
            signingCredentials: credenciales);

        var tokenGenerado =
            new JwtSecurityTokenHandler().WriteToken(token);

        var respuesta = new LoginResponseDTO
        {
            Token = tokenGenerado,
            Expira = fechaExpiracion
        };

        return Task.FromResult<LoginResponseDTO?>(respuesta);
    }
}
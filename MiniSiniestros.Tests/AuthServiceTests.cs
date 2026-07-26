using Microsoft.Extensions.Configuration;
using MiniSiniestros.Dto;
using MiniSiniestros.Services;

namespace MiniSiniestros.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task Login_CredencialesValidas_DevuelveToken()
    {
        // Arrange
        var datosConfiguracion = new Dictionary<string, string?>
        {
            ["Jwt:Clave"] =
                "MiniSiniestros-Clave-Super-Secreta-2026-Desarrollo",

            ["Jwt:Emisor"] =
                "MiniSiniestros.Api",

            ["Jwt:Audiencia"] =
                "MiniSiniestros.Clientes",

            ["Jwt:ExpiracionMinutos"] =
                "60"
        };

        IConfiguration configuracion =
            new ConfigurationBuilder()
                .AddInMemoryCollection(datosConfiguracion)
                .Build();

        var servicio = new AuthService(configuracion);

        var request = new LoginRequestDTO
        {
            Usuario = "operador",
            Password = "operador123"
        };

        // Act
        var resultado = await servicio.LoginAsync(request);

        // Assert
        Assert.NotNull(resultado);
        Assert.False(string.IsNullOrWhiteSpace(resultado.Token));
        Assert.True(resultado.Expira > DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_PasswordIncorrecta_DevuelveNull()
    {
        // Arrange
        var configuracion = CrearConfiguracion();

        var servicio = new AuthService(configuracion);

        var request = new LoginRequestDTO
        {
            Usuario = "operador",
            Password = "password-incorrecta"
        };

        // Act
        var resultado = await servicio.LoginAsync(request);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task Login_UsuarioIncorrecto_DevuelveNull()
    {
        // Arrange
        var configuracion = CrearConfiguracion();

        var servicio = new AuthService(configuracion);

        var request = new LoginRequestDTO
        {
            Usuario = "usuario-incorrecto",
            Password = "operador123"
        };

        // Act
        var resultado = await servicio.LoginAsync(request);

        // Assert
        Assert.Null(resultado);
    }

    private static IConfiguration CrearConfiguracion()
    {
        var datosConfiguracion = new Dictionary<string, string?>
        {
            ["Jwt:Clave"] =
                "MiniSiniestros-Clave-Super-Secreta-2026-Desarrollo",

            ["Jwt:Emisor"] =
                "MiniSiniestros.Api",

            ["Jwt:Audiencia"] =
                "MiniSiniestros.Clientes",

            ["Jwt:ExpiracionMinutos"] =
                "60"
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(datosConfiguracion)
            .Build();
    }
}
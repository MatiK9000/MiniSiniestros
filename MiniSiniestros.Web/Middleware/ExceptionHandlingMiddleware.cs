using MiniSiniestros.Services;
using MiniSiniestros.Services.Exceptions;

namespace MiniSiniestros.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ReglaNegocioException ex)
        {
            logger.LogWarning(
                ex,
                "Se produjo un error de negocio: {Mensaje}",
                ex.Message);

            context.Response.Redirect(
                $"/Error/Negocio?mensaje={Uri.EscapeDataString(ex.Message)}");
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Se produjo un error inesperado.");

            context.Response.Redirect("/Error/General");
        }
    }
}
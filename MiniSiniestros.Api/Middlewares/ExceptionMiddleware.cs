using MiniSiniestros.Services.Exceptions;
using System.Net;
using System.Text.Json;

namespace MiniSiniestros.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                    "La petición fue rechazada por una regla de negocio: {Mensaje}",
                    ex.Message);

                await EscribirRespuestaAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrió un error inesperado.");

                await EscribirRespuestaAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno en el servidor.");
            }
        }

        private static async Task EscribirRespuestaAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string mensaje)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = new
            {
                mensaje
            };

            string json = JsonSerializer.Serialize(respuesta);

            await context.Response.WriteAsync(json);
        }
    }
}

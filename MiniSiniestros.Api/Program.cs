using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniSiniestros.Api.Mappings;
using MiniSiniestros.Api.Middlewares;
using MiniSiniestros.Data;
using MiniSiniestros.Services;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errores = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors
                    .Select(error =>
                        string.IsNullOrWhiteSpace(error.ErrorMessage)
                            ? "El valor ingresado no es válido."
                            : error.ErrorMessage)
                    .ToArray());

        var respuesta = new
        {
            status = StatusCodes.Status400BadRequest,
            mensaje = "Error de validación.",
            errores
        };

        return new BadRequestObjectResult(respuesta);
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions =>
            sqlServerOptions.MigrationsAssembly(
                "MiniSiniestros.Data.Migrations")));

builder.Services.AddAutoMapper(
    config => { },
    typeof(SiniestroProfile));

builder.Services.AddScoped<ISiniestroService, SiniestroService>();
builder.Services.AddScoped<ITrabajadorService, TrabajadorService>();
builder.Services.AddScoped<IEmpleadorService, EmpleadorService>();
builder.Services.AddScoped<IPrestadorMedicoService, PrestadorMedicoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

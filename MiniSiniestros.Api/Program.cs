using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniSiniestros.Api.Mappings;
using MiniSiniestros.Api.Middlewares;
using MiniSiniestros.Data;
using MiniSiniestros.Services;
using Serilog;
using Serilog.Events;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var claveJwt = builder.Configuration["Jwt:Clave"] ?? throw new InvalidOperationException("No se configuró la clave JWT.");

var emisorJwt = builder.Configuration["Jwt:Emisor"] ?? throw new InvalidOperationException("No se configuró el emisor JWT.");

var audienciaJwt = builder.Configuration["Jwt:Audiencia"] ?? throw new InvalidOperationException("No se configuró la audiencia JWT.");

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


// Add services to the container.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = emisorJwt,

                ValidateAudience = true,
                ValidAudience = audienciaJwt,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(claveJwt)),

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy(
        "SoloOperadores",
        politica => politica.RequireRole("Operador"));
});

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
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese: Bearer {token}"
    });

    opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

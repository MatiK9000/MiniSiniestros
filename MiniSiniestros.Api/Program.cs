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

using Microsoft.EntityFrameworkCore;
using MiniSiniestros.Data;
using MiniSiniestros.Services;
using MiniSiniestros.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión DefaultConnection.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ISiniestroService, SiniestroService>();
builder.Services.AddScoped<IEmpleadorService, EmpleadorService>();
builder.Services.AddScoped<ITrabajadorService, TrabajadorService>();
builder.Services.AddScoped<IPrestadorMedicoService, PrestadorMedicoService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Siniestros}/{action=Index}/{id?}");

app.Run();
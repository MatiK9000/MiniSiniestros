using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;

namespace MiniSiniestros.Api.Controllers;

[ApiController]
[Route("api/empleadores")]
public class EmpleadoresController : ControllerBase
{
    private readonly IEmpleadorService empleadorService;
    private readonly IMapper mapper;

    public EmpleadoresController(
        IEmpleadorService empleadorService,
        IMapper mapper)
    {
        this.empleadorService = empleadorService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmpleadorDto>>> Get()
    {
        var empleadores = await empleadorService.ObtenerTodosAsync();

        var empleadoresDto =
            mapper.Map<IEnumerable<EmpleadorDto>>(empleadores);

        return Ok(empleadoresDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmpleadorDto>> GetPorId(int id)
    {
        var empleador = await empleadorService.ObtenerPorIdAsync(id);

        if (empleador is null)
        {
            return NotFound(new
            {
                mensaje = "El empleador no fue encontrado."
            });
        }

        var empleadorDto = mapper.Map<EmpleadorDto>(empleador);

        return Ok(empleadorDto);
    }

    [HttpPost]
    public async Task<ActionResult<EmpleadorDto>> Post(
        CrearEmpleadorDto crearEmpleadorDto)
    {
        var empleador = mapper.Map<Empleador>(crearEmpleadorDto);

        var empleadorCreado =
            await empleadorService.CrearAsync(empleador);

        var empleadorDto =
            mapper.Map<EmpleadorDto>(empleadorCreado);

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = empleadorDto.Id },
            empleadorDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(
        int id,
        ModificarEmpleadorDto modificarEmpleadorDto)
    {
        var empleador =
            mapper.Map<Empleador>(modificarEmpleadorDto);

        await empleadorService.ModificarAsync(id, empleador);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await empleadorService.EliminarAsync(id);

        return NoContent();
    }
}
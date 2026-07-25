using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;

namespace MiniSiniestros.Api.Controllers;

[ApiController]
[Route("api/trabajadores")]
public class TrabajadoresController : ControllerBase
{
    private readonly ITrabajadorService trabajadorService;
    private readonly IMapper mapper;

    public TrabajadoresController(ITrabajadorService trabajadorService, IMapper mapper)
    {
        this.trabajadorService = trabajadorService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrabajadorDto>>> Get()
    {
        var trabajadores = await trabajadorService.ObtenerTodosAsync();

        var trabajadoresDto =
            mapper.Map<IEnumerable<TrabajadorDto>>(trabajadores);

        return Ok(trabajadoresDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrabajadorDto>> GetPorId(int id)
    {
        var trabajador = await trabajadorService.ObtenerPorIdAsync(id);

        if (trabajador is null)
        {
            return NotFound(new
            {
                mensaje = "El trabajador no fue encontrado."
            });
        }

        var trabajadorDto = mapper.Map<TrabajadorDto>(trabajador);

        return Ok(trabajadorDto);
    }

    [HttpPost]
    public async Task<ActionResult<TrabajadorDto>> Post(
        CrearTrabajadorDto crearTrabajadorDto)
    {
        var trabajador =
            mapper.Map<Trabajador>(crearTrabajadorDto);

        var trabajadorCreado =
            await trabajadorService.CrearAsync(trabajador);

        var trabajadorDto =
            mapper.Map<TrabajadorDto>(trabajadorCreado);

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = trabajadorDto.Id },
            trabajadorDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(
        int id,
        ModificarTrabajadorDto modificarTrabajadorDto)
    {
        var trabajador =
            mapper.Map<Trabajador>(modificarTrabajadorDto);

        await trabajadorService.ModificarAsync(id, trabajador);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await trabajadorService.EliminarAsync(id);

        return NoContent();
    }
}
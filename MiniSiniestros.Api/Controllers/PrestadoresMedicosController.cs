using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;

namespace MiniSiniestros.Api.Controllers;

[ApiController]
[Route("api/prestadoresmedicos")]
public class PrestadoresMedicosController : ControllerBase
{
    private readonly IPrestadorMedicoService prestadorMedicoService;
    private readonly IMapper mapper;

    public PrestadoresMedicosController(IPrestadorMedicoService prestadorMedicoService, IMapper mapper)
    {
        this.prestadorMedicoService = prestadorMedicoService;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrestadorMedicoDto>>> Get()
    {
        var prestadores = await prestadorMedicoService.ObtenerTodosAsync();

        var prestadoresDto =
            mapper.Map<IEnumerable<PrestadorMedicoDto>>(prestadores);

        return Ok(prestadoresDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PrestadorMedicoDto>> GetPorId(int id)
    {
        var prestador = await prestadorMedicoService.ObtenerPorIdAsync(id);

        if (prestador is null)
        {
            return NotFound(new
            {
                mensaje = "El prestador médico no fue encontrado."
            });
        }

        var prestadorDto = mapper.Map<PrestadorMedicoDto>(prestador);

        return Ok(prestadorDto);
    }

    [HttpPost]
    public async Task<ActionResult<PrestadorMedicoDto>> Post(
        CrearPrestadorMedicoDto crearPrestadorMedicoDto)
    {
        var prestador =
            mapper.Map<PrestadorMedico>(crearPrestadorMedicoDto);

        var prestadorCreado =
            await prestadorMedicoService.CrearAsync(prestador);

        var prestadorDto =
            mapper.Map<PrestadorMedicoDto>(prestadorCreado);

        return CreatedAtAction(
            nameof(GetPorId),
            new { id = prestadorDto.Id },
            prestadorDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(
        int id,
        ModificarPrestadorMedicoDto modificarPrestadorMedicoDto)
    {
        var prestador =
            mapper.Map<PrestadorMedico>(modificarPrestadorMedicoDto);

        await prestadorMedicoService.ModificarAsync(id, prestador);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await prestadorMedicoService.EliminarAsync(id);

        return NoContent();
    }
}
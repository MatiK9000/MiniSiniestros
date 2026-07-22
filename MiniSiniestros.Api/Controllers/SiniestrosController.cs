using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;
using MiniSiniestros.Services;

namespace MiniSiniestros.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiniestrosController : ControllerBase
    {
        private readonly ISiniestroService siniestroService;
        private readonly IMapper mapper;

        public SiniestrosController(ISiniestroService siniestroService, IMapper mapper)
        {
            this.siniestroService = siniestroService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<SiniestroDto>> Crear(CrearSiniestroDto dto)
        {
            Siniestro siniestro = mapper.Map<Siniestro>(dto);

            await siniestroService.CrearAsync(siniestro);

            SiniestroDto siniestroDto =
                mapper.Map<SiniestroDto>(siniestro);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = siniestro.Id },
                siniestroDto);
        }

        [HttpPost("{id:int}/prestadores/{prestadorId:int}")]
        public async Task<ActionResult> AsignarPrestador(int id, int prestadorId)
        {
            bool resultado = await siniestroService.AsignarPrestadorAsync(id, prestadorId);

            if (!resultado)
            {
                return BadRequest(new
                {
                    mensaje = "No fue posible asignar el prestador."
                });
            }

            return NoContent();
        }

        [HttpPut("{id:int}/estado")]
        public async Task<ActionResult> CambiarEstado(int id, EstadoSiniestro nuevoEstado)
        {
            bool resultado = await siniestroService.CambiarEstadoAsync(id, nuevoEstado);

            if (!resultado)
            {
                return NotFound(new
                {
                    mensaje = $"No se encontró el siniestro con ID {id}."
                });
            }

            return NoContent();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiniestroDto>>> ObtenerTodos()
        {
            var siniestros = await siniestroService.ObtenerTodosAsync();

            var siniestrosDto = mapper.Map<IEnumerable<SiniestroDto>>(siniestros);

            return Ok(siniestrosDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SiniestroDto>> ObtenerPorId(int id)
        {
            var siniestro = await siniestroService.ObtenerPorIdAsync(id);

            if (siniestro is null)
            {
                return NotFound(new
                {
                    mensaje = $"No se encontró el siniestro con ID {id}."
                });
            }

            var siniestroDto = mapper.Map<SiniestroDto>(siniestro);

            return Ok(siniestroDto);
        }


    }
}

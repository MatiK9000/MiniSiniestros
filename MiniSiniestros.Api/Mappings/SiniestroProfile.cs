using AutoMapper;
using MiniSiniestros.Dto;
using MiniSiniestros.Entities;


namespace MiniSiniestros.Api.Mappings
{
    public class SiniestroProfile : Profile
    {
        public SiniestroProfile()
        {
            CreateMap<CrearSiniestroDto, Siniestro>();

            CreateMap<Siniestro, SiniestroDto>()
            .ForMember(
                dto => dto.EmpleadorRazonSocial,
                config => config.MapFrom(s => s.Empleador.RazonSocial))
            .ForMember(
                dto => dto.TrabajadorNombreCompleto,
                config => config.MapFrom(
                    s => $"{s.Trabajador.Nombre} {s.Trabajador.Apellido}"));
        }
    }
}
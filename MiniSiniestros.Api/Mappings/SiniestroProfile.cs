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

            CreateMap<CrearEmpleadorDto, Empleador>();

            CreateMap<ModificarEmpleadorDto, Empleador>();

            CreateMap<Empleador, EmpleadorDto>();

            CreateMap<CrearTrabajadorDto, Trabajador>();

            CreateMap<ModificarTrabajadorDto, Trabajador>();

            CreateMap<Trabajador, TrabajadorDto>();

            CreateMap<CrearPrestadorMedicoDto, PrestadorMedico>();

            CreateMap<ModificarPrestadorMedicoDto, PrestadorMedico>();

            CreateMap<PrestadorMedico, PrestadorMedicoDto>();
        }
    }
}
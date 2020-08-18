using AutoMapper;
using RallyDakar.API.Models;
using RallyDakar.Dominio.Entidades;

namespace RallyDakar.API.AutoMapper
{
    public class TelemetriaProfile : Profile
    {
        public TelemetriaProfile()
        {
            CreateMap<Telemetria, TelemetriaModelo>().ReverseMap();
        }
    }
}

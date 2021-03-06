﻿using AutoMapper;
using RallyDakar.API.Models;
using RallyDakar.Dominio.Entidades;

namespace RallyDakar.API.AutoMapper
{
    public class PilotoProfile : Profile
    {
        public PilotoProfile()
        {
            CreateMap<Piloto, PilotoModelo>().ReverseMap();
        }
    }
}

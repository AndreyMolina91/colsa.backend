
using AutoMapper;
using COLSA.Domain.Models;
using COLSA.Infraestructure.Dtos;

namespace COLSA.Infraestructure.Mapping
{
    public class MappingConfiguration
    {
        public static MapperConfiguration RegisterMaps()
        {
            // Para generar los mapeos entre DTOs y Modelos para la insercion en DB
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<UserRegisterDto, UserModel>().ReverseMap();
                config.CreateMap<TournamentDto, TournamentModel>().ReverseMap();
            });

            return mapping;
        }
    }
}
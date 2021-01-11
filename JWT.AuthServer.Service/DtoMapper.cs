using AutoMapper;
using JWT.AuthServer.Core.DTOs;
using JWT.AuthServer.Core.Entities;
using System;

namespace JWT.AuthServer.Service
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}

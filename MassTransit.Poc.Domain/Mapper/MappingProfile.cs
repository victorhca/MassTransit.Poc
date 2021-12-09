using AutoMapper;
using MassTransit.Poc.Domain.Dto;
using MassTransit.Poc.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Poc.Domain.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewVehicleDto, NewVehicle>().ReverseMap();
        }
    }
}

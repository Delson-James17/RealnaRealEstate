using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.DTO;
using RealEstate.API.Models;

namespace RealEstate.API.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<EstateProperty, EstatePropertyDto>().ReverseMap();
        }
        
    }
}

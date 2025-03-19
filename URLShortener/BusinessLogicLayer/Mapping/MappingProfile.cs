using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using BusinessLogicLayer.DTO;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Url, UrlDTO>();
            CreateMap<UrlDTO, Url>();
        }
    }
}

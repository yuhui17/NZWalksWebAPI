using AutoMapper;
using NZWalks.Model.Models.Domain;
using NZWalks.Model.Models.DTOs;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }
    }
}

using AutoMapper;
using System.Linq;

namespace PhotoMasterBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Label, DTOs.Label>();
            CreateMap<DTOs.Label, Models.Label>();
            CreateMap<Models.Photo, DTOs.Photo>()
                .ForMember(destination => destination.Labels, 
                map => map.MapFrom(source => source.PhotoLabels.Select(pl => pl.Label)));
            CreateMap<DTOs.Photo, Models.Photo>();
        }
    }
}

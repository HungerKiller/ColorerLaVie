using AutoMapper;

namespace PhotoMasterBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Label, DTOs.Label>();
            CreateMap<DTOs.Label, Models.Label>();
            CreateMap<Models.Photo, DTOs.Photo>();
            CreateMap<DTOs.Photo, Models.Photo>();
        }
    }
}

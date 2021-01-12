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
            CreateMap<DTOs.Photo, Models.Photo>()
                .ForMember(destination => destination.PhotoLabels,
                map => map.MapFrom(source => source.Labels.Select(l => new Models.PhotoLabel() { PhotoId = source.Id, LabelId = l.Id})));
            // todo 新建时，Photo的Id都是0。但是在添加PhotoLabels时，EF会自动找到PhotoLabels中的PhotoId，所以此处即使写了source.Id + 999，EF也会自动识别到对的PhotoId
            // 更新时，原理一样
            // 因为这个工作是EF做的，所以为了准确性，可能需要手动验证？
        }
    }
}

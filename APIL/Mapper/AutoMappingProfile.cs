using AutoMapper;
using BusinessObject.Entities;
using BusinessObject.Model;

namespace WebAppAPI.Mapper
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile() {
            CreateMap<Category, CategoryReturnDTO>()
                .ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentCategory));

            CreateMap<Category, ParentCategoryDTO>();
        }
    }
}

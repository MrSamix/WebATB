using AutoMapper;
using WebATB.Data.Entities;
using WebATB.Models.Category;
using WebATB.Models.Helpers;

namespace WebATB.Mappers;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<CategoryEntity,CategoryItemModel>()
            .ForMember(x=>x.Image, 
                opt => opt.MapFrom(x=>string.IsNullOrEmpty(x.Image) ? $"/images/noimage.png" : $"/images/200_{x.Image}"));

        CreateMap<CategoryCreateModel, CategoryEntity>()
            .ForMember(x=>x.Image, opt => opt.Ignore());

        CreateMap<CategoryEntity, CategoryUpdateModel>()
            .ForMember(x => x.Image,
                opt => opt.MapFrom(x => string.IsNullOrEmpty(x.Image) ? $"/images/noimage.png" : $"/images/200_{x.Image}"));

        CreateMap<CategoryUpdateModel, CategoryEntity>()
           .ForMember(x => x.Image, opt => opt.Ignore());

        CreateMap<CategoryEntity, SelectItemHelper>();
    }
}

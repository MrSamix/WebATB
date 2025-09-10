using AutoMapper;
using Microsoft.AspNetCore.Connections.Features;
using WebATB.Data.Entities;
using WebATB.Models.Product;

namespace WebATB.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<ProductCreateModel, ProductEntity>();
        CreateMap<ProductEntity, ProductItemModel>()
            .ForMember(x => x.ProductImages, 
            opt => opt.MapFrom(x => x.ProductsImages!
            .OrderBy(x => x.Priority)
            .Select(pi => $"/images/200_{pi.Path}").ToList()))
            .ForMember(opt => opt.CategoryName, prop => prop.MapFrom(x=>x.Category.Name));
        CreateMap<ProductEntity, ProductUpdateModel>()
            .ForMember(x => x.Images,
            opt => opt.MapFrom(x => x.ProductsImages!
            .OrderBy(x => x.Priority)
            .Select(pi => $"/images/200_{pi.Path}").ToList()))
            .ForMember(opt => opt.CategoryId, prop => prop.MapFrom(x => x.Category.Id));
        CreateMap<ProductUpdateModel, ProductEntity>();
    }
}

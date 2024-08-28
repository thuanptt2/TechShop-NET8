using AutoMapper;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Application.Models.Categories;
using TechShopSolution.Application.Models.Products;
using TechShopSolution.Application.Models.Brands;

namespace TechShopSolution.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductInCategory != null ? src.ProductInCategory
                .Where(pc => pc.Product != null).Select(pc => pc.Product): new List<Product>()));

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand));
            // .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductInCategory != null ? src.ProductInCategory
            //     .Where(pc => pc.Category != null).Select(pc => pc.Category): new List<Category>()));

            CreateMap<Brand, BrandDTO>();
        }
    }
}
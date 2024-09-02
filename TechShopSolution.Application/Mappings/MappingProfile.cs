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
            //Category
            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductInCategory!.Where(pc => pc.Product != null).Select(pc => pc.Product)));
            CreateMap<Product, CategoryDTO.ProductDTO>();

            //CategoryProduct
            CreateMap<CategoryProductDTO, CategoryProduct>();

            //Product
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductInCategory!.Where(pc => pc.Category != null).Select(pc => pc.Category)));
            CreateMap<CreateProductDTO, Product>();
            CreateMap<Category, ProductDTO.CategoryDTO>();

            //Brand
            CreateMap<Brand, BrandDTO>();

        }
    }
}
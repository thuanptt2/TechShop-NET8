using AutoMapper;
using TechShopSolution.Domain.Entities;
using TechShopSolution.Domain.Models.Categories;
using TechShopSolution.Domain.Models.Products;
using TechShopSolution.Domain.Models.Brands;
using TechShopSolution.Application.Commands.Products.CreateProduct;
using TechShopSolution.Application.Commands.Products.UpdateProduct;
using TechShopSolution.Domain.Models;
using TechShopSolution.Application.Events;

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

            //Product
            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductInCategory!.Where(pc => pc.Category != null).Select(pc => pc.Category)));
            CreateMap<Category, ProductDTO.CategoryDTO>();

            CreateMap<CreateProductCommand, Product>();
            CreateMap<CreateProductCommand, MongoProduct>();
            CreateMap<CreateProductCommand, ProductCreatedEvent>();
            CreateMap<UpdateProductCommand, Product>();
            CreateMap<Product, ProductCreatedEvent>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductInCategory!.Select(pc => pc.CateId)));
            CreateMap<Product, ProductUpdatedEvent>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductInCategory!.Select(pc => pc.CateId)));
            CreateMap<ProductCreatedEvent, MongoProduct>();
            CreateMap<ProductUpdatedEvent, MongoProduct>();

            //CategoryProduct
            CreateMap<CategoryProductDTO, CategoryProduct>();

            //Brand
            CreateMap<Brand, BrandDTO>();

        }
    }
}
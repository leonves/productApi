using AutoMapper;
using ProductApi.Models.Request;
using ProductApi.Models.Response;

namespace ProductApi.AutoMapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<ProductRequest, Product>();
            CreateMap<Category, CategoryRequest>();
            CreateMap<Product, ProductRequest>();
        }
    }
}

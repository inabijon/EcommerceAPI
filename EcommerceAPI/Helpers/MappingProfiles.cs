using AutoMapper;
using Core.Entities;
using EcommerceAPI.Dtos;

namespace EcommerceAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ProductPicture, o => o.MapFrom<ProductUrlResolver>());
        }
    }
}
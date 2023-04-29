using AutoMapper;
using Core.Entities;
using EcommerceAPI.Dtos;

namespace EcommerceAPI.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ProductPicture))
            {
                return _config["ApiUrl"] + source.ProductPicture;
            }

            return null;
        }
    }
}
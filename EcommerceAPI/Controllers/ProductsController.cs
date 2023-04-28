using System.Linq;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using EcommerceAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        public ProductsController(IGenericRepository<Product> productRepo, 
                                IGenericRepository<ProductBrand> productBrandRepo, 
                                IGenericRepository<ProductType> productTypeRepo)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithBrandsAndTypesSpecification();
            var products = await _productRepo.ListAsync(spec);
            return products.Select(product => new ProductToReturnDto{
                Id = product.Id,
                Name = product.Name,
                Description= product.Description,
                ProductPicture = product.ProductPicture,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name 
            }).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithBrandsAndTypesSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);

            if(product == null) 
            {
                return NotFound();
            }

            return new ProductToReturnDto {
                Id = product.Id,
                Name = product.Name,
                Description= product.Description,
                ProductPicture = product.ProductPicture,
                ProductBrand = product.ProductBrand.Name,
                ProductType = product.ProductType.Name 
            };
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductsBrands() {
            var brands = await _productBrandRepo.ListAllAsync();
            return Ok(brands);
        }   
        
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() {
            var types = await _productTypeRepo.ListAllAsync();
            return Ok(types);
        }
    }
}
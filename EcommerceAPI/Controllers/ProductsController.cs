using System.Linq;
using AutoMapper;
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

        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepo, 
                                IGenericRepository<ProductBrand> productBrandRepo, 
                                IGenericRepository<ProductType> productTypeRepo,
                                IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithBrandsAndTypesSpecification();
            var products = await _productRepo.ListAsync(spec);
           
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
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

            return _mapper.Map<Product, ProductToReturnDto>(product);
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
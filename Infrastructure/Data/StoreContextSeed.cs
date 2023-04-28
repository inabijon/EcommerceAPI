using System.Text.Json;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreSeedContext 
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory) 
        {
            try
            {
                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("./SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var brand in brands)
                    {
                        context.ProductBrands.Add(brand);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("./SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var type in types)
                    {
                        context.ProductTypes.Add(type);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productData = File.ReadAllText("./SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex) {
                var logger = loggerFactory.CreateLogger<StoreSeedContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}
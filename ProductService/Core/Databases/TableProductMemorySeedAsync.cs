using Microsoft.EntityFrameworkCore;
using ProductService.Core.Databases.InMemory;

namespace ProductService.Core.Databases
{
    public class TableProductMemorySeedAsync
    {
        public async Task SeedAsync(TableProductMemory memory, ApplicationDbContext context)
        {
            var products = await context.TableProducts.ToListAsync();

            if(products.Count > 0)
            {
                foreach(var product in products)
                {
                    memory.ProductMem.Add(product.Id.ToString(), product);
                }
            }
        }
    }
}

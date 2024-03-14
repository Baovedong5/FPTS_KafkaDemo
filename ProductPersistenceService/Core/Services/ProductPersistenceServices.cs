using ProductPersistenceService.Core.Databases;
using ProductPersistenceService.Core.IServices;
using ProductPersistenceService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceService.Core.Services
{
    internal class ProductPersistenceServices : IProductPersistenceService
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<ProductPersistenceServices> _logger;

        public ProductPersistenceServices(ApplicationDbContext context, ILogger<ProductPersistenceServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TableProduct> InsertProduct(TableProduct product)
        {
            try
            {
                await _context.TableProducts.AddAsync(product);

                await _context.SaveChangesAsync();

                return product;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<TableProduct> UpdatePrice(int productId, decimal price)
        {
            var product = await _context.TableProducts.FindAsync(productId);

            product.Price = price;

            _context.TableProducts.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<TableProduct> UpdateQuantity(int productId, int quantity)
        {
            var product = await _context.TableProducts.FindAsync(productId);

            product.Quantity = quantity;

            _context.TableProducts.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }
    }
}

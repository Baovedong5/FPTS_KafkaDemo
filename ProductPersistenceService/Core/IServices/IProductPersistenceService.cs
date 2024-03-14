using ProductPersistenceService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductPersistenceService.Core.IServices
{
    internal interface IProductPersistenceService
    {
        Task<TableProduct> InsertProduct(TableProduct product);

        Task<TableProduct> UpdatePrice(int productId, decimal price);

        Task<TableProduct> UpdateQuantity(int productId, int quantity);
    }
}

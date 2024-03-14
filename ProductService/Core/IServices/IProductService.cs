using ProductService.Core.Models;

namespace ProductService.Core.IServices
{
    public interface IProductService
    {
        List<TableProduct> ListAsync();

        TableProduct InsertAsync(TableProduct product);

        TableProduct UpdatePriceAsync(int productId, decimal price);

        TableProduct UpdateQuantityAsync(int productId, int quantity, bool increase);
    }
}

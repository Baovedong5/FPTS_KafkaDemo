using ProductService.Core.Models;

namespace ProductService.Core.Databases.InMemory
{
    public class TableProductMemory
    {
        public Dictionary<string, TableProduct> ProductMem { get; set; }

        public TableProductMemory()
        {
            ProductMem = new Dictionary<string, TableProduct>();
        }
    }
}

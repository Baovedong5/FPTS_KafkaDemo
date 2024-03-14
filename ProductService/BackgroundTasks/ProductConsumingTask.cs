using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using ProductService.Core.IServices;
using ProductService.Core.Models;
using System.Text.Json;
using System.Text;
using ProductService.DTOs;

namespace ProductService.BackgroundTasks
{
    public class ProductConsumingTask : IConsumingTask<string, string>
    {
        private readonly IProductService _productService;

        public ProductConsumingTask(IProductService productService)
        {
            _productService = productService;
        }

        public void Execute(ConsumeResult<string, string> result)
        {
            var productEvent = "";

            foreach (var header in result.Message.Headers)
            {
                productEvent = Encoding.UTF8.GetString(header.GetValueBytes());
            }

            if (productEvent == "InsertProduct")
            {
                var product = JsonSerializer.Deserialize<TableProduct>(result.Message.Value);
                _productService.InsertAsync(product);
            }
            else if (productEvent == "UpdateQuantity")
            {
                var product = JsonSerializer.Deserialize<UpdateQuantityDto>(result.Message.Value);
                _productService.UpdateQuantityAsync(product.Id, product.Quantity, product.Increase);
            }
            else if (productEvent == "UpdatePrice")
            {
                var product = JsonSerializer.Deserialize<UpdatePriceDto>(result.Message.Value);
                _productService.UpdatePriceAsync(product.Id, product.Price);
            }

        }
    }
}

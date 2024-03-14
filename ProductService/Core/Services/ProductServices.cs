using Confluent.Kafka;
using Manonero.MessageBus.Kafka.Abstractions;
using ProductService.Core.Databases.InMemory;
using ProductService.Core.IServices;
using ProductService.Core.Models;
using System.Text;
using System.Text.Json;

namespace ProductService.Core.Services
{
    public class ProductServices : IProductService
    {

        private readonly TableProductMemory _inMem;

        private readonly ILogger<ProductServices> _logger;

        private readonly IKafkaProducerManager _producerManager;

        public ProductServices(TableProductMemory inMem, ILogger<ProductServices> logger, IKafkaProducerManager producerManager)
        {
            _inMem = inMem;
            _logger = logger;
            _producerManager = producerManager;
        }

        public TableProduct InsertAsync(TableProduct product)
        {
            try
            {
                _inMem.ProductMem.Add(product.Id.ToString(), product);

                var kafkaProducer = _producerManager.GetProducer<string, string>("1");

                var message = new Message<string, string>
                {
                    Key = "emptyornull",
                    Value = JsonSerializer.Serialize(product),
                    Headers = new Headers
                    {
                        {
                            "eventname", Encoding.UTF8.GetBytes("InsertProduct")
                        }
                    }
                };

                kafkaProducer.Produce(message);

                return product;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public List<TableProduct> ListAsync()
        {
            try
            {
                var products = _inMem.ProductMem.Values.ToList();

                return products;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public TableProduct UpdatePriceAsync(int productId, decimal price)
        {
            try
            {
                var product = _inMem.ProductMem.FirstOrDefault(x => x.Value.Id == productId).Value;

                if(product != null)
                {
                    _inMem.ProductMem.TryGetValue(productId.ToString(), out product);

                    if(price < 0)
                    {
                        _logger.LogError("price must be greater than 0");
                    }
                    else
                    {
                        product.Price = price;

                        var kafkaProducer = _producerManager.GetProducer<string, string>("1");

                        var message = new Message<string, string>
                        {
                            Key = productId.ToString(),
                            Value = JsonSerializer.Serialize(product),
                            Headers = new Headers
                            {
                                {
                                    "eventname", Encoding.UTF8.GetBytes("UpdatePrice")
                                }
                            }
                        };

                        kafkaProducer.Produce(message);

                    }

                    return product;
                }
                else
                {
                    _logger.LogError($"Product {productId} is not exist");
                    throw new Exception($"Product {productId} is not exist");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public TableProduct UpdateQuantityAsync(int productId, int quantity, bool increase)
        {
            try
            {
                var product = _inMem.ProductMem.FirstOrDefault(x => x.Value.Id == productId).Value;

                if(product != null)
                {
                    _inMem.ProductMem.TryGetValue(productId.ToString(), out product);

                    if(increase)
                    {
                        product.Quantity += quantity;
                    }
                    else
                    {
                        product.Quantity -= quantity;
                    }

                    if(product.Quantity < 0)
                    {
                        _logger.LogError("negative quantity");
                    }
                    else
                    {
                        var kafkaProducer = _producerManager.GetProducer<string, string>("1");

                        var message = new Message<string, string>
                        {
                            Key = productId.ToString(),
                            Value = JsonSerializer.Serialize(product),
                            Headers = new Headers
                            {
                               {
                                  "eventname", Encoding.UTF8.GetBytes("UpdateQuantity")
                               },
                            }
                        };

                        kafkaProducer.Produce(message);
                    }

                    return product;
                }
                else
                {
                    _logger.LogError("product is not exist");
                    throw new Exception("product is not exist");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}

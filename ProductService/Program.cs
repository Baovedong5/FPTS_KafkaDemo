using Manonero.MessageBus.Kafka.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductService.BackgroundTasks;
using ProductService.Core.Databases;
using ProductService.Core.Databases.InMemory;
using ProductService.Core.IServices;
using ProductService.Core.Services;
using ProductService.Extensions;
using ProductService.Settings;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

var appSetting = AppSetting.MapValue(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(connection);
});

builder.Services.AddSingleton<TableProductMemory>();

builder.Services.AddSingleton<IProductService, ProductServices>();


builder.Services.AddKafkaConsumers(builder =>
{
    builder.AddConsumer<ProductConsumingTask>(appSetting.GetConsumerSetting("0"));
});

builder.Services.AddKafkaProducers(builder =>
{
    builder.AddProducer(appSetting.GetProducerSetting("1"));
});

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.LoadDataToMemory<TableProductMemory, ApplicationDbContext>((data, context) =>
{
    new TableProductMemorySeedAsync().SeedAsync(data, context).Wait();
});

app.UseKafkaMessageBus(mess =>
{
    mess.RunConsumer("0");
});

app.Run();

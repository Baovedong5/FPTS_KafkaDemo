using Manonero.MessageBus.Kafka.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductPersistenceService;
using ProductPersistenceService.BackgroundTasks;
using ProductPersistenceService.Core.Databases;
using ProductPersistenceService.Core.IServices;
using ProductPersistenceService.Core.Services;
using ProductPersistenceService.Settings;

var builder = Host.CreateApplicationBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

var appSetting = AppSetting.MapValue(builder.Configuration);

builder.Services.AddHostedService<Worker>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseOracle(connection);
//});

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseOracle(connection);
builder.Services.AddSingleton<ApplicationDbContext>(new ApplicationDbContext(optionsBuilder.Options));

builder.Services.AddSingleton<IProductPersistenceService, ProductPersistenceServices>();

builder.Services.AddKafkaConsumers(builder =>
{
    builder.AddConsumer<ProductPersistenceConsumingTask>(appSetting.GetConsumerSetting("1"));
});


var host = builder.Build();

host.UseKafkaMessageBus(mess =>
{
    mess.RunConsumer("1");
});

host.Run();

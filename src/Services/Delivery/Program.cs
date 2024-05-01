using Delivery.Entities;
using Delivery.MongoDb;
using MassTransit;
using System.Reflection;
using Delivery.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(Assembly.GetEntryAssembly());
    x.UsingRabbitMq((context, configurator) =>
    {
        var configuration = context.GetService<IConfiguration>();
        var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMqSettings!.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings!.ServiceName, false));
        configurator.UseMessageRetry(b =>
        {
            b.Interval(3, TimeSpan.FromSeconds(5));
        });
    });
});

builder.Services.AddMongo().AddMongoRepository<CourierDelivery>("courierDeliveryItems").AddMongoRepository<Order>("orderItems");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

using MassTransit;
using Ordering.Entities;
using Ordering.Hubs;
using Ordering.MongoDb;
using Ordering.Processors;
using Ordering.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(Assembly.GetEntryAssembly());
    x.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMqSettings!.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings!.ServiceName, false));
    });
});

builder.Services.AddMongo().AddMongoRepository<Order>("orderItems");

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddSignalR();

builder.Services.AddScoped<OrderingProcessor>();

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

app.MapHub<RealTimeOrderStatusHub>("/order-status");

app.Run();

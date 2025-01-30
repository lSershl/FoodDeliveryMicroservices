var builder = DistributedApplication.CreateBuilder(args);

var identityDb = builder.AddPostgres("fdm-identity-db")
    .WithDataVolume()
    .WithPgAdmin();

var redisCache = builder.AddRedis("fdm-redis-cache")
    .WithDataVolume();

var mongoDb = builder.AddMongoDB("fdm-mongo-db")
    .WithDataVolume();

var rabbitMq = builder.AddRabbitMQ("fdm-rabbit-mq")
    .WithDataVolume()
    .WithManagementPlugin();

var apiGateway = builder.AddProject<Projects.YARPGateway>("yarp-gateway");

builder.AddProject<Projects.WebClient>("web-client")
    .WithReference(apiGateway);

builder.AddProject<Projects.Identity>("identity")
    .WithReference(identityDb);

builder.AddProject<Projects.Basket>("basket")
    .WithReference(redisCache)
    .WithReference(rabbitMq);

builder.AddProject<Projects.Catalog>("catalog")
    .WithReference(mongoDb)
    .WithReference(rabbitMq);

builder.AddProject<Projects.Ordering>("ordering")
    .WithReference(mongoDb)
    .WithReference(rabbitMq);

builder.AddProject<Projects.Delivery>("delivery")
    .WithReference(mongoDb)
    .WithReference(rabbitMq);

builder.AddProject<Projects.Identity_Migration>("identity-migrations")
    .WithReference(identityDb);

builder.Build().Run();

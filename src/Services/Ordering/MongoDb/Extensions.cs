﻿using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using Ordering.Entities;
using Ordering.Settings;

namespace Ordering.MongoDb
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            services.AddSingleton(a =>
            {
                var configuration = a.GetService<IConfiguration>();
                var serviceSettings = configuration!.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(configuration.GetConnectionString("fdm-mongo-db"));
                return mongoClient.GetDatabase(serviceSettings!.ServiceName);
            });
            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(a =>
            {
                var database = a.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database!, collectionName);
            });
            return services;
        }
    }
}

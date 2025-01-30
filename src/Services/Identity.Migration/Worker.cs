using Identity.Data;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace Identity.Migration;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        Guid testUserId = Guid.NewGuid();

        User testUser = new()
        {
            CustomerId = testUserId,
            PhoneNumber = "+79012223344",
            Password = "test",
            Name = "Test User",
            Birthday = DateTime.UtcNow.Date,
            Email = "test@gmail.com"
        };

        Address testAddress = new()
        {
            Id = Guid.NewGuid(),
            CustomerId = testUserId,
            City = "Братск",
            Street = "Советская",
            House = "1",
            Apartment = "1",
            FullAddress = "Братск, Советская 1-1"
        };

        PaymentCard testPaymentCard = new()
        {
            Id = Guid.NewGuid(),
            CustomerId = testUserId,
            CardHolderName = "IVAN IVANOV",
            CardNumber = "1111 2222 3333 4444",
            Expiration = "10/25",
            Cvv = "111"
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            if (dbContext.Users.Any() is false)
            {
                await dbContext.Users.AddAsync(testUser, cancellationToken);
            }
            if (dbContext.SavedUserAddresses.Any() is false)
            {
                await dbContext.SavedUserAddresses.AddAsync(testAddress, cancellationToken);
            }
            if (dbContext.SavedUserPaymentCards.Any() is false)
            {
                await dbContext.SavedUserPaymentCards.AddAsync(testPaymentCard, cancellationToken);
            }
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}

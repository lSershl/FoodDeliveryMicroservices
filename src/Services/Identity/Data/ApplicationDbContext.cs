using Identity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Identity.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One User can have many saved addresses for delivery
            builder.Entity<User>()
                .HasMany(u => u.SavedAddresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.CustomerId)
                .IsRequired();

            // One User can have many saved payment cards
            builder.Entity<User>()
                .HasMany(u => u.SavedPaymentCards)
                .WithOne(pc => pc.User)
                .HasForeignKey(pc => pc.CustomerId)
                .IsRequired();

            // Seed test user data

            Guid testUserId = Guid.NewGuid();

            builder.Entity<User>().HasData(new User
            {
                CustomerId = testUserId,
                PhoneNumber = "+79012223344",
                Password = "test",
                Name = "Test User",
                Birthday = DateTime.UtcNow.Date,
                Email = "test@gmail.com"
            });

            builder.Entity<Address>().HasData(new Address
            {
                Id = Guid.NewGuid(),
                CustomerId = testUserId,
                City = "Братск",
                Street = "Советская",
                House = "1",
                Apartment = "1",
                FullAddress = "Братск, Советская 1-1"
            });

            builder.Entity<PaymentCard>().HasData(new PaymentCard
            {
                Id = Guid.NewGuid(),
                CustomerId = testUserId,
                CardHolderName = "IVAN IVANOV",
                CardNumber = "1111 2222 3333 4444",
                Expiration = "10/25",
                Cvv = "111"
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> SavedUserAddresses { get; set; }
        public DbSet<PaymentCard> SavedUserPaymentCards { get; set; }
    }
}

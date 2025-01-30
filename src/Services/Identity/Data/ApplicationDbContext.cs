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

            // One user can have many saved addresses for delivery
            builder.Entity<User>()
                .HasMany(u => u.SavedAddresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.CustomerId)
                .IsRequired();

            // One user can have many saved payment cards
            builder.Entity<User>()
                .HasMany(u => u.SavedPaymentCards)
                .WithOne(pc => pc.User)
                .HasForeignKey(pc => pc.CustomerId)
                .IsRequired();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> SavedUserAddresses { get; set; }
        public DbSet<PaymentCard> SavedUserPaymentCards { get; set; }
    }
}

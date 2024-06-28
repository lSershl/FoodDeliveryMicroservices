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

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                CustomerId = Guid.NewGuid(),
                PhoneNumber = "+79012223344",
                Password = "test",
                Name = "Test User",
                Address = "Советская 1-1",
                Birthday = DateTime.UtcNow.Date,
                Email = "test@gmail.com"
            });
        }

        public DbSet<ApplicationUser> Users { get; set; }
    }
}

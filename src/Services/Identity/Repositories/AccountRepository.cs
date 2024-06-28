using Identity.Data;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class AccountRepository(ApplicationDbContext dbContext) : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<ApplicationUser> GetUserByPhoneAsync(string phoneNumber)
        {
            var user = await _dbContext.Users.FirstAsync(x => x.PhoneNumber == phoneNumber);
            return user!;
        }

        public void RegisterUser(ApplicationUser user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}

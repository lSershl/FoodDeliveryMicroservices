using Identity.Data;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class AccountRepository(ApplicationDbContext dbContext) : IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<User> GetUserByPhoneAsync(string phoneNumber)
        {
            var user = await _dbContext.Users.FirstAsync(x => x.PhoneNumber == phoneNumber);
            return user;
        }

        public void RegisterUser(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}

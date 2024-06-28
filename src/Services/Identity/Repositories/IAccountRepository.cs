using Identity.Entities;

namespace Identity.Repositories
{
    public interface IAccountRepository
    {
        Task<ApplicationUser> GetUserByPhoneAsync(string phoneNumber);
        void RegisterUser(ApplicationUser user);
    }
}

using Identity.Entities;

namespace Identity.Repositories
{
    public interface IAccountRepository
    {
        Task<User> GetUserByPhoneAsync(string phoneNumber);
        void RegisterUser(User user);
    }
}

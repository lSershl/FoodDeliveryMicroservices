using Identity.Entities;

namespace Identity.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesByUserAsync(Guid customerId);
        void AddAddress(Address address);
        void RemoveAddress(Guid customerId, string address);
    }
}

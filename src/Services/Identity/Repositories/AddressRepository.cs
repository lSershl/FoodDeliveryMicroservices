using Identity.Data;
using Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Repositories
{
    public class AddressRepository(ApplicationDbContext dbContext) : IAddressRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<Address>> GetAddressesByUserAsync(Guid customerId)
        {
            var addresses = await _dbContext.SavedUserAddresses.Where(x => x.CustomerId == customerId).ToListAsync();
            return addresses;
        }

        public void AddAddress(Address address)
        {
            _dbContext.SavedUserAddresses.Add(address);
            _dbContext.SaveChanges();
        }

        public void RemoveAddress(Guid customerId, string address)
        {
            var userAddresses = _dbContext.SavedUserAddresses.Where(x => x.CustomerId == customerId).ToList();

            var addressToDelete = userAddresses
                .Find(x => x.FullAddress.Equals(address));

            if (addressToDelete is not null)
            {
                _dbContext.SavedUserAddresses.Remove(addressToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}

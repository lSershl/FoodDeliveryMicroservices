using Identity.Entities;
using Identity.Infrastructure;
using Identity.Repositories;
using Identity.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("addresses")]
    public class AddressController(IAddressRepository addressRepository) : ControllerBase
    {
        private readonly IAddressRepository _addressRepository = addressRepository;

        [HttpGet("for_customer/{customerId}")]
        [Authorize]
        public async Task<ActionResult> GetUserAddresses(Guid customerId)
        {
            List<SavedAddressDto> userAddresses = new();
            var result = await _addressRepository.GetAddressesByUserAsync(customerId);
            foreach (var address in result)
            {
                userAddresses.Add(new SavedAddressDto(
                    address.City + " " + 
                    address.Street + " " + 
                    address.House + " " + 
                    address.Apartment));
            }
            return Ok(userAddresses);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SaveNewAddressForUser(NewAddressDto newAddressDto)
        {
            _addressRepository.AddAddress(new Address 
            {
                Id = Guid.NewGuid(),
                CustomerId = newAddressDto.CustomerId,
                City = newAddressDto.City,
                Street = newAddressDto.Street,
                House = newAddressDto.House,
                Apartment = newAddressDto.Apartment,
                FullAddress = newAddressDto.City + ", " + newAddressDto.Street + " " + newAddressDto.House + "-" + newAddressDto.Apartment
            });
            return Ok("Адрес сохранён");
        }

        [HttpDelete("{customerId}")]
        [Authorize]
        public async Task<ActionResult> DeleteSavedAddress(Guid customerId, string address)
        {
            _addressRepository.RemoveAddress(customerId, address);
            return Ok("Адрес удалён");
        }
    }
}

using Catalog.Data;
using Catalog.Entities;
using Catalog.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("catalog")]
    public class CatalogController(IRepository<CatalogItem> repository, CatalogInitialData initialData) : ControllerBase
    {
        private readonly IRepository<CatalogItem> _repository = repository;
        private readonly CatalogInitialData _initialData = initialData;

        [HttpGet]
        public async Task<ActionResult> GetCatalogAsync()
        {
            var items = (await _repository.GetAllAsync()).Select(a => a.AsDto());
            if (items.Count() == 0)
            {
                var initialProductsList = _initialData.GetInitialData();
                foreach (var product in initialProductsList)
                {
                    await _repository.CreateAsync(product);
                }
                var initialItems = (await _repository.GetAllAsync()).Select(a => a.AsDto());
                return Ok(initialItems);
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCatalogItemByIdAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound("Товар не найден!");
            }
            return Ok(item!.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult> PostCatalogItemAsync(CreateCatalogItemDto createCatalogItemDto)
        {
            var catalogItem = new CatalogItem
            {
                Name = createCatalogItemDto.Name,
                Description = createCatalogItemDto.Description,
                PictureUrl = createCatalogItemDto.PictureUrl,
                Price = createCatalogItemDto.Price
            };
            await _repository.CreateAsync(catalogItem);
            return CreatedAtAction(nameof(GetCatalogItemByIdAsync), new { id = catalogItem.Id }, catalogItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCatalogItemAsync(Guid id, UpdateCatalogItemDto updateCatalogItemDto)
        {
            var existingCatalogItem = await _repository.GetAsync(id);
            if (existingCatalogItem is null)
            {
                return NotFound("Товар не найден!");
            }
            existingCatalogItem.Name = updateCatalogItemDto.Name;
            existingCatalogItem.Description = updateCatalogItemDto.Description;
            existingCatalogItem.PictureUrl = updateCatalogItemDto.PictureUrl;
            existingCatalogItem.Price = updateCatalogItemDto.Price;
            await _repository.UpdateAsync(existingCatalogItem);
            return Ok("Товар сохранён");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCatalogItemAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item is null)
            {
                return NotFound("Товар не существует!");
            }
            await _repository.RemoveAsync(item.Id);
            return Ok("Товар удалён");
        }
    }
}

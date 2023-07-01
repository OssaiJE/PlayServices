using CatalogService.Entities;
using CatalogService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository = new();

        // GET: api/<ItemsController>
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAllItems()
        {
            var items = (await itemsRepository.GetAllItems()).Select(item => item.AsDto());

            return items;
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemById(Guid id)
        {
            var item = await itemsRepository.GetOneItem(id);
            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        // POST api/<ItemsController>
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto createItemDto)
        {
            var item = new ItemEntity
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateItem(item);

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetOneItem(id);
            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateItem(existingItem);

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var item = await itemsRepository.GetOneItem(id);

            if (item == null)
            {
                return NotFound();
            }
            await itemsRepository.RemoveItem(item.Id);

            return NoContent();
        }
    }
}

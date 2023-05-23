using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Test 1", "Test 1 description", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Test 2", "Test 2 description", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Test 3", "Test 3 description", 9, DateTimeOffset.UtcNow)
        };

        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<ItemDto> GetAllItems()
        {
            return items;
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItemById(Guid id)
        {
            var item = items.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto createItemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateItem(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(item => item.Id == id).FirstOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price,
            };

            var itemIndex = items.FindIndex(existingItem => existingItem.Id == id);
            items[itemIndex] = updatedItem;

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteItem(Guid id)
        {
            var itemIndex = items.FindIndex(existingItem => existingItem.Id == id);
            if (itemIndex < 0)
            {
                return NotFound();
            }
            items.RemoveAt(itemIndex);

            return NoContent();
        }
    }
}

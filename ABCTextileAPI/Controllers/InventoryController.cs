using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

[Authorize]

[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly ABCTextileContext _context;

    public InventoryController(ABCTextileContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventories()
    {
        var inventories = await _context.Inventories.ToListAsync();
        return Ok(inventories);
    }

    [HttpPost]
    public async Task<IActionResult> AddInventory(Inventory inventory)
    {
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetInventories), new { id = inventory.InventoryId }, inventory);
    }
}

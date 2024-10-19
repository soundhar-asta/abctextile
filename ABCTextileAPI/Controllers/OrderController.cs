using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ABCTextileContext _context;

    public OrderController(ABCTextileContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _context.Orders.Include(o => o.Inventory).ToListAsync();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order);
    }
}

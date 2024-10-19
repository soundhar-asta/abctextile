using Microsoft.AspNetCore.Mvc;
using ABCTextileApp.Models;
using ABCTextileApp.Data;

namespace ABCTextileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ABCTextileContext _context;

        public OrderController(ABCTextileContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(o => o.Inventory).ToListAsync();
        }
    }
}

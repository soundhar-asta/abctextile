using Microsoft.EntityFrameworkCore;

public class ABCTextileContext : DbContext
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ABCTextileContext(DbContextOptions<ABCTextileContext> options)
        : base(options) { }

}

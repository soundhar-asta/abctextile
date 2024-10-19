using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext for Entity Framework Core with SQL Server
builder.Services.AddDbContext<ABCTextileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ABCTextileConnection")));

// Add authentication services for JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ValidateIssuer = true, // Uncomment if you want to validate the issuer
        // ValidateAudience = true, // Uncomment if you want to validate the audience
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // ValidIssuer = "yourissuer", // Uncomment if you want to set a valid issuer
        // ValidAudience = "youraudience", // Uncomment if you want to set a valid audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yoursecretkey")) // Keep the security key for now
    };
});

// Add authorization services
builder.Services.AddAuthorization();

// Add application services (e.g., Controllers)
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Define routes (for Inventory and Orders)

app.MapGet("/api/inventory", async (ABCTextileContext db) =>
{
    return await db.Inventories.ToListAsync();
})
.WithName("GetInventory")
.WithOpenApi();

app.MapPost("/api/inventory", async (ABCTextileContext db, Inventory inventory) =>
{
    db.Inventories.Add(inventory);
    await db.SaveChangesAsync();
    return Results.Created($"/api/inventory/{inventory.InventoryId}", inventory);
})
.WithName("CreateInventory")
.WithOpenApi();

app.MapGet("/api/orders", async (ABCTextileContext db) =>
{
    return await db.Orders.Include(o => o.Inventory).ToListAsync();
})
.WithName("GetOrders")
.WithOpenApi();

app.MapPost("/api/orders", async (ABCTextileContext db, Order order) =>
{
    db.Orders.Add(order);
    await db.SaveChangesAsync();
    return Results.Created($"/api/orders/{order.OrderId}", order);
})
.WithName("CreateOrder")
.WithOpenApi();

app.MapGet("/api/hello", () => "Welcome to the ABC Textile API!"); // Test endpoint

app.Run();

// Models

public record Inventory
{
    public int InventoryId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}

public record Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}

// Data Context

public class ABCTextileContext : DbContext
{
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ABCTextileContext(DbContextOptions<ABCTextileContext> options)
        : base(options) { }
}

// UserService for JWT Token Generation
public class UserService
{
    public string Authenticate(string username, string password)
    {
        // Simulate a simple user check (you can extend this as needed)
        if (username == "admin" && password == "password")
        {
            return GenerateJwtToken(username);
        }
        return null;
    }

    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("yoursecretkey");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}

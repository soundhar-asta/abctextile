public class Order
#pragma warning restore CA1050 // Declare types in namespaces
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }
}

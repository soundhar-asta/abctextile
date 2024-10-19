public class StockMovement
{
    public int InventoryStockMovementId { get; set; }
    public int InventoryId { get; set; }
    public Inventory Inventory { get; set; }

    public int QuantityMoved { get; set; } // Can be positive or negative
    public DateTime DateOfMovement { get; set; }

    public string Reason { get; set; } // E.g., "Sale", "Restock", "Adjustment"
}

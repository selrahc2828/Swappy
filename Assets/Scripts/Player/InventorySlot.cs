
[System.Serializable]
public class InventorySlot
{
    public ItemData item; // faire avec une classe parent ? 
    public int quantity;

    public InventorySlot( ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

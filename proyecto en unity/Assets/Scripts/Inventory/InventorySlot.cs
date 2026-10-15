using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;
    public int wrongQuantity; // Cuántas de las 'quantity' unidades son falsas.

    public InventorySlot(ItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
    }
}

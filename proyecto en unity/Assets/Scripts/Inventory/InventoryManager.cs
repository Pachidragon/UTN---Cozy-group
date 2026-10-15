using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance {  get; private set; }
    [SerializeField] private int inventorySize = 10;
    private InventorySlot[] slots;

    public Action OnInventoryChanged;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        slots = new InventorySlot[inventorySize];
    }

    public bool AddItem(ItemData itemData, int quantity, bool wrong = false)
    {
        int max = Mathf.Max(1, itemData.maxStackSize);

        // Verifica que entre todo antes de modificar nada.
        int capacity = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) capacity += max;
            else if (slots[i].itemData == itemData) capacity += max - slots[i].quantity;
        }
        if (capacity < quantity) return false;

        int remaining = quantity;

        // Completa los slots que ya tienen este item.
        for (int i = 0; i < slots.Length && remaining > 0; i++)
        {
            if (slots[i] == null || slots[i].itemData != itemData) continue;

            int toAdd = Mathf.Min(max - slots[i].quantity, remaining);
            slots[i].quantity += toAdd;
            if (wrong) slots[i].wrongQuantity += toAdd;
            remaining -= toAdd;
        }

        // El resto va a slots vacíos.
        for (int i = 0; i < slots.Length && remaining > 0; i++)
        {
            if (slots[i] != null) continue;

            int toAdd = Mathf.Min(max, remaining);
            slots[i] = new InventorySlot(itemData, toAdd);
            if (wrong) slots[i].wrongQuantity = toAdd;
            remaining -= toAdd;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }
}

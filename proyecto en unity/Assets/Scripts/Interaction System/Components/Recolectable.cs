using UnityEngine;
// Controla la interacción y recolección de objetos del escenario.
public enum AnimationContext { GrabDown, GrabMid, Drink }
public class Recolectable : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;

    [Header("Behaviour")]
    [SerializeField] private bool destroyOnPickUp = true;
    private bool pickedUp = false;
    [SerializeField] private bool wrongItem = false; // Indica si el objeto es un elemento incorrecto (se guarda en el inventario como falso).
    [SerializeField] private bool notStored = false;    // Item falso. Se recoge y se destruye, pero no entra al inventario.

    [Header("Contexto de Animación")]
    [SerializeField] private AnimationContext animationContext = AnimationContext.GrabDown;
    public AnimationContext CurrentContext => animationContext;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;
    public bool WrongItem => wrongItem;


    public void Interact()
    {
        if (!TryPickUp()) return;

        OnPickedUp();
    }

    private bool TryPickUp()
    {
        if (pickedUp) return false;
        if (itemData == null && !notStored) return false; // Solo los que se guardan necesitan ItemData.

        pickedUp = true;
        return true;
    }

    private void OnPickedUp()
    {
        if (itemData != null && (itemData.itemID == "Pocion" || itemData.itemID == "Item_Potion"))
        {
            if (QuestTrackerUI.Instance != null)
            {
                QuestTrackerUI.Instance.MarcarPocionBebida();
                Debug.Log("[Misiones]: Señal de poción enviada con éxito desde Recolectable.");
            }
        }

        if (!notStored)
        {
            bool added = InventoryManager.Instance.AddItem(itemData, quantity, wrongItem);

            if (!added)
            {
                pickedUp = false; // Inventario lleno, se puede reintentar.
                return;
            }
        }

        Debug.Log(itemData != null ? $"Recogido:{itemData.itemName} x{quantity}" : "Recogido: item falso");

        if (destroyOnPickUp)
            Destroy(gameObject);
    }
}

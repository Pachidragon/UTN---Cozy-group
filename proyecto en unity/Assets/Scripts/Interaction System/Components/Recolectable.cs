using UnityEngine;
// Controla la interacción y recolección de objetos del escenario.
public class Recolectable : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;

    [Header("Behaviour")]
    [SerializeField] private bool destroyOnPickUp = true;
    private bool pickedUp = false;
    [SerializeField] private bool wrongItem = false; // Indica si el objeto es un elemento incorrecto.
    
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
        if (pickedUp || itemData == null) return false;

        pickedUp = true;
        return true;
    }

    private void OnPickedUp()
    {
        Debug.Log("Recogido" + itemData.itemName + "x " + quantity);

        if (destroyOnPickUp)
            Destroy(gameObject);
    }
}

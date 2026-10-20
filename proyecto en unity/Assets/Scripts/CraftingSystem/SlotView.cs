using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Casillero visual, se usa tanto para el inventario como para los huecos del círculo.
[RequireComponent(typeof(Button))]
public class SlotView : MonoBehaviour
{
    [SerializeField] private Image icon;       // Ícono del item.
    [SerializeField] private TMP_Text label;   // Nombre y cantidad.

    public Button Button { get; private set; }

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    // Muestra un item y su cantidad.
    public void Set(ItemData data, int quantity, bool interactable)
    {
        if (icon != null)
        {
            icon.sprite = data.icon; // Ajustá el nombre si tu campo no se llama 'icon'.
            icon.enabled = data.icon != null;
        }
        if (label != null) label.text = data.itemName + " x" + quantity;

        Button.interactable = interactable;
    }

    // Deja el casillero vacío.
    public void Clear()
    {
        if (icon != null) icon.enabled = false;
        if (label != null) label.text = "";

        Button.interactable = false;
    }
}

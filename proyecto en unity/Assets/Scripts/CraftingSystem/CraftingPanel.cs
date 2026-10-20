using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Panel de crafteo, muestra el inventario, los huecos del círculo y gestiona los intentos.
public class CraftingPanel : MonoBehaviour
{
    public static CraftingPanel Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelRoot;        // Raíz que se activa/desactiva (hijo de este objeto).
    [SerializeField] private Transform inventoryContainer; // Padre de los casilleros del inventario.
    [SerializeField] private SlotView inventorySlotPrefab; // Prefab de un casillero del inventario.
    [SerializeField] private SlotView[] craftSlots;        // Los 4 huecos del círculo.
    [SerializeField] private Button craftButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text messageText;         // Mensajes al jugador.
    [SerializeField] private GameObject resultIcon;        // Se activa en el centro cuando sale la poción.

    [Header("Reglas")]
    [SerializeField] private int maxAttempts = 3; // Intentos compartidos entre los dos calderos.

    [Header("Eventos")]
    [SerializeField] private UnityEvent onOpened; // Desactivar el movimiento y liberar el cursor.
    [SerializeField] private UnityEvent onClosed; // Reactivar el movimiento.

    private readonly List<SlotView> inventoryViews = new List<SlotView>();
    private readonly List<int> placed = new List<int>(); // Índices de slots del inventario puestos en el círculo.
    private Cauldron currentCauldron;
    private int attemptsLeft;
    private bool success; // El crafteo actual ya salió bien.

    public bool IsOpen => panelRoot != null && panelRoot.activeSelf;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        attemptsLeft = maxAttempts;
    }

    private void Start()
    {
        // Crea un casillero por cada slot del inventario.
        InventorySlot[] slots = InventoryManager.Instance.Slots;
        for (int i = 0; i < slots.Length; i++)
        {
            int index = i; // Copia local para el listener.
            SlotView view = Instantiate(inventorySlotPrefab, inventoryContainer);
            view.Button.onClick.AddListener(() => OnInventorySlotClicked(index));
            inventoryViews.Add(view);
        }

        // Cada hueco del círculo devuelve su stack al inventario al hacer clic.
        for (int j = 0; j < craftSlots.Length; j++)
        {
            int index = j;
            craftSlots[j].Button.onClick.AddListener(() => OnCraftSlotClicked(index));
        }

        craftButton.onClick.AddListener(OnCraftPressed);
        closeButton.onClick.AddListener(Close);
        InventoryManager.Instance.OnInventoryChanged += Refresh;

        panelRoot.SetActive(false);
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
    }

    // Abre el panel para un caldero concreto.
    public void Open(Cauldron cauldron)
    {
        if (IsOpen) return; // Evita reabrir si el jugador vuelve a pulsar E.

        currentCauldron = cauldron;
        success = false;
        placed.Clear();
        SetMessage("");
        if (resultIcon != null) resultIcon.SetActive(false);

        panelRoot.SetActive(true);
        Refresh();
        onOpened?.Invoke();
    }

    // Cierra el panel. Si el crafteo salió bien, el caldero instancia la poción.
    public void Close()
    {
        panelRoot.SetActive(false);

        if (success && currentCauldron != null) currentCauldron.SpawnPotion();

        currentCauldron = null;
        onClosed?.Invoke();
    }

    // Clic en un stack del inventario, va al primer hueco libre del círculo.
    private void OnInventorySlotClicked(int slotIndex)
    {
        if (success) return;

        InventorySlot slot = InventoryManager.Instance.Slots[slotIndex];
        if (slot == null || placed.Contains(slotIndex)) return;

        if (placed.Count >= craftSlots.Length)
        {
            SetMessage("No hay más espacios disponibles.");
            return;
        }

        placed.Add(slotIndex);
        Refresh();
    }

    // Clic en un hueco del círculo, el stack vuelve al inventario.
    private void OnCraftSlotClicked(int craftIndex)
    {
        if (success || craftIndex >= placed.Count) return;

        placed.RemoveAt(craftIndex);
        Refresh();
    }

    // Botón "Craftear" valida el intento y aplica el resultado.
    private void OnCraftPressed()
    {
        if (success || placed.Count == 0) return;

        InventoryManager inventory = InventoryManager.Instance;
        CraftResult result = CraftingLogic.Evaluate(placed, inventory.Slots, currentCauldron.Recipe);

        switch (result)
        {
            case CraftResult.Success:
                // Consume solo la cantidad que pide la receta de cada stack.
                foreach (int index in placed)
                {
                    InventorySlot slot = inventory.Slots[index];
                    foreach (Cauldron.Ingredient ingredient in currentCauldron.Recipe)
                    {
                        if (ingredient.item == slot.itemData)
                        {
                            inventory.RemoveFromSlot(index, ingredient.amount);
                            break;
                        }
                    }
                }

                success = true;
                placed.Clear();
                if (resultIcon != null) resultIcon.SetActive(true);
                SetMessage("¡Poción creada! Cerrá el panel para recogerla.");
                Refresh();
                break;

            case CraftResult.WrongItem:
                // Elimina solo las unidades incorrectas de los stacks usados, las verdaderas se quedan.
                foreach (int index in placed) inventory.RemoveWrongFromSlot(index);
                Fail("Parece que se ha intentado usar un item incorrecto...");
                break;

            case CraftResult.WrongRecipe:
                Fail("Receta incorrecta.");
                break;
        }
    }

    // Registra un intento fallido. Si se acaban los intentos, activa la derrota.
    private void Fail(string message)
    {
        attemptsLeft--;
        placed.Clear();
        Refresh();

        if (attemptsLeft <= 0)
        {
            Close();
            if (GameCode.Instance != null) GameCode.Instance.LoseOn(); // Panel de derrota.
            return;
        }

        SetMessage(message + " Intentos restantes: " + attemptsLeft);
    }

    // Redibuja el inventario y los huecos del círculo.
    private void Refresh()
    {
        if (!IsOpen) return;

        InventorySlot[] slots = InventoryManager.Instance.Slots;
        for (int i = 0; i < inventoryViews.Count; i++)
        {
            if (slots[i] == null) inventoryViews[i].Clear();
            else inventoryViews[i].Set(slots[i].itemData, slots[i].quantity, !placed.Contains(i)); // Los usados se atenúan.
        }

        for (int j = 0; j < craftSlots.Length; j++)
        {
            if (j < placed.Count && slots[placed[j]] != null)
                craftSlots[j].Set(slots[placed[j]].itemData, slots[placed[j]].quantity, true);
            else
                craftSlots[j].Clear();
        }

        craftButton.interactable = !success && placed.Count > 0;
    }

    private void SetMessage(string text)
    {
        if (messageText != null) messageText.text = text;
    }
}

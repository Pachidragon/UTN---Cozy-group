using System.Collections.Generic;
using UnityEngine;

// Caldero interactuable. Al interactuar abre el panel de crafteo con su propia receta.
public class Cauldron : MonoBehaviour, IInteractable
{
    // Un ingrediente de la receta: qué item y cuántas unidades hacen falta.
    [System.Serializable]
    public class Ingredient
    {
        public ItemData item;
        public int amount = 1;
    }

    [Header("Receta")]
    [SerializeField] private List<Ingredient> recipe = new List<Ingredient>(); // Cada caldero define la suya.

    [Header("Resultado")]
    [SerializeField] private GameObject prefabPotion; // Prefab de la poción que se va a crear.
    [SerializeField] private Transform pivotSpawn;    // Punto donde aparecerá la poción.

    private bool crafted = false; // Indica si el caldero ya creó la poción.

    public List<Ingredient> Recipe => recipe;

    // Lo llama PlayerInteractor a través de la interfaz.
    public void Interact()
    {
        if (crafted)
        {
            Debug.Log("[Caldero]: El caldero está vacío.");
            return;
        }

        if (CraftingPanel.Instance == null)
        {
            Debug.LogWarning("No hay un CraftingPanel en la escena.");
            return;
        }

        CraftingPanel.Instance.Open(this);
    }

    // Lo llama el panel al cerrarse después de un crafteo exitoso.
    public void SpawnPotion()
    {
        if (crafted) return;
        crafted = true; // Evita crear otra poción.

        Debug.Log("[Caldero]: Instanciando poción");

        if (prefabPotion != null && pivotSpawn != null)
        {
            Instantiate(prefabPotion, pivotSpawn.position, pivotSpawn.rotation);
            Debug.Log("Objeto instanciado bien");
        }
    }
}

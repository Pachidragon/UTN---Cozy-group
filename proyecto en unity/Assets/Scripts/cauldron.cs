using UnityEngine;
// Comportamiento del caldero.
// Hereda de NPCBase para poder utilizar el sistema de interacción/Talk().
public class cauldron : NPCBase
{

    [SerializeField] private GameObject prefabPotion;  // Prefab de la poción que se va a crear.
    [SerializeField] private Transform pivotSpawn;  // Punto de referencia donde aparecerá la poción.

    private bool Crafted = false; // Indica si el caldero ya creó la poción.
    public override void Talk() // Se ejecuta cuando el jugador interactúa con el caldero.
    {
        if (Crafted)  // Si la poción ya fue creada, el caldero no vuelve a crear otra.
        {
            Debug.Log("[" + nameNPC + "]: El caldero está vacío.");
            return;
        }

        Debug.Log("[" + nameNPC + "]: Instanciando poción"); // Informa en la consola que se va a crear la poción.
        CraftingPotion();
    }

    private void CraftingPotion() // Crea e instancia la poción en el punto indicado.
    {
        Crafted = true; // Marca el caldero como utilizado para evitar crear otra poción.

        if (prefabPotion != null && pivotSpawn != null)
        {
            Instantiate(prefabPotion, pivotSpawn.position, pivotSpawn.rotation);
            Debug.Log("Objeto instanciado bien"); // Confirma en la consola que la instancia fue creada.
        }
    }
}

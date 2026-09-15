using UnityEngine;

public class cauldron : NPCBase
{
    [SerializeField] private GameObject prefabPotion;
    [SerializeField] private Transform pivotSpawn;

    private bool Crafted = false;
    public override void Talk()
    {
        if (Crafted)
        {
            Debug.Log("[" + nameNPC + "]: El caldero está vacío.");
            return;
        }

        Debug.Log("[" + nameNPC + "]: Instanciando poción");
        CraftingPotion();
    }

    private void CraftingPotion()
    {
        Crafted = true;

        if (prefabPotion != null && pivotSpawn != null)
        {
            Instantiate(prefabPotion, pivotSpawn.position, pivotSpawn.rotation);
            Debug.Log("[Sistema]: Objeto instanciado bien");
        }
    }
}

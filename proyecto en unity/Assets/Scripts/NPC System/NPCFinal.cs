using UnityEngine;
// Controla la interacción con el NPC que desencadena la victoria del juego (Mirtha)

public class NPCVictoria : NPCBase
{
    [SerializeField] private GameObject prefabItemFinal;
    [SerializeField] private Transform pivotSpawnItemFinal;

    private bool win = false; // Indica si la victoria ya fue otorgada.

    public override void Talk() // Ejecuta el diálogo del NPC 
    {
        if (win) return;

        base.Talk();
        OtorgarVictoria();
    }

    private void OtorgarVictoria()  // Entrega el objeto final y activa el estado de victoria.
    {
        win = true;

        if (prefabItemFinal != null && pivotSpawnItemFinal != null)
        {
            Instantiate(prefabItemFinal, pivotSpawnItemFinal.position, pivotSpawnItemFinal.rotation);
        }

        if (GameCode.Instance != null)
        {
            GameCode.Instance.VictoryOn(); // Activa el panel de victoria.
        }
    }
}
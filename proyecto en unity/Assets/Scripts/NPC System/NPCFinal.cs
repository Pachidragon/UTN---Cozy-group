using UnityEngine;

public class NPCVictoria : NPCBase
{
    [SerializeField] private GameObject prefabItemFinal;
    [SerializeField] private Transform pivotSpawnItemFinal;
    [SerializeField] private Sprite avatarMirtha; 

    private bool win = false; // Indica si la victoria ya fue otorgada.

    public override void Talk() // Ejecuta el diálogo del NPC 
    {
        if (win || Dialogues.Instance == null) return; //

        // Lee el Element 0 que le configures en el Inspector a Mirtha.
        string textoDialogo = (dialogues != null && dialogues.Length > 0) ? dialogues[0] : "¡Felicidades, has completado tu entrenamiento!";
        Dialogues.Instance.ShowDialogue(nameNPC, textoDialogo, avatarMirtha, this);

        VictoryDone();
    }

    private void VictoryDone()  // Entrega el objeto final y activa el estado de victoria.
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

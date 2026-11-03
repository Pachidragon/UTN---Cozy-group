using UnityEngine;

public class NPCVictoria : NPCBase
{
    [SerializeField] private GameObject prefabItemFinal; // Prefab de la yerba mate
    [SerializeField] private Transform pivotSpawnItemFinal; // Dónde aparece la yerba
    [SerializeField] private Sprite avatarMirtha;

    private int indiceDialogoActual = 0; // Controla qué parte del discurso largo se muestra
    private bool terminoDeHablar = false; // Bloquea el diálogo para que no se repita el discurso

    public override void Talk()
    {
        if (Dialogues.Instance == null) return;

        // Si ya terminó de hablar por completo y le vuelve a pulsar E, muestra un recordatorio corto
        if (terminoDeHablar)
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "Lleva la yerba hacia el mate ceremonial para completar tu entrenamiento, querida.", avatarMirtha, this);
            return;
        }
        if (dialogues != null && dialogues.Length > 0)
        {
            if (indiceDialogoActual < dialogues.Length)
            {
                // Muestra la frase correspondiente al paso actual
                Dialogues.Instance.ShowDialogue(nameNPC, dialogues[indiceDialogoActual], avatarMirtha, this);
                indiceDialogoActual++; // Avanza a la siguiente parte para la próxima pulsación de E
            }
            else
            {
                // Se quedó sin frases en el arreglo: finaliza el discurso y entrega el ítem
                EntregarYerbaMate();
            }
        }
        else
        {
            Dialogues.Instance.ShowDialogue(nameNPC, "¡Felicidades por llegar hasta mí, jovencita! Has demostrado un gran talento en tu viaje. Toma esta Yerba Mate sagrada.", avatarMirtha, this);
            EntregarYerbaMate();
        }
    }

    private void EntregarYerbaMate()
    {
        terminoDeHablar = true;
        Dialogues.Instance.ShowDialogue(nameNPC, "Coloca la yerba en el mate ceremonial para recibir tu bendición final.", avatarMirtha, this);

        if (prefabItemFinal != null && pivotSpawnItemFinal != null)
        {
            Instantiate(prefabItemFinal, pivotSpawnItemFinal.position, pivotSpawnItemFinal.rotation);
            Debug.Log("[Final]: Mirtha te entregó la Yerba Mate.");
        }

        if (QuestTrackerUI.Instance != null)
        {
            QuestTrackerUI.Instance.UpdateQuestPanel();
        }
    }
}


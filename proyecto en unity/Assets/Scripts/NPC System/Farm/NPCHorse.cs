using UnityEngine;
// Controla la interacción con el caballo y actualiza el progreso de la misión.
public class NPCHorse : NPCBase
{
    [SerializeField] private float distanciaParaHablar = 3f;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update() // Comprueba la distancia al jugador y permite iniciar la interacción.
    {
        if (playerTransform == null) return;
        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanciaParaHablar)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }

    public override void Talk() // Muestra el diálogo del caballo y actualiza el progreso de la misión.
    {
        if (FarmQuestManager.Instance != null && !FarmQuestManager.Instance.talkToFarmer)
        {
            Debug.Log("[" + nameNPC + "]: *Bufido* La granjera te busca, joven aprendiz.");
            return;
        }

        base.Talk(); // Ejecuta el diálogo configurado en NPC
        if (FarmQuestManager.Instance != null)
        {
            FarmQuestManager.Instance.talkToHorse = true; // Registra que el jugador habló con el caballo.
        }
    }
}


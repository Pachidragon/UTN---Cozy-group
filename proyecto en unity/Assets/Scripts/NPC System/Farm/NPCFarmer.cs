using UnityEngine;
// Controla la interacción con la granjera y modifica su diálogo según el progreso de la misión.

public class NPCFarmer : NPCBase
{
    [SerializeField] private string[] dialoguesAfterHorse;
    [SerializeField] private float distanciaParaHablar = 3f;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    void Update()
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

    public override void Talk() // Muestra un diálogo diferente según si el jugador ya habló con el caballo.
    {
        if (FarmQuestManager.Instance != null && FarmQuestManager.Instance.talkToHorse)
        {
            Debug.Log("[" + nameNPC + "]: Estuve intentando recordar, pero aun no estoy segura. Pensaba que se la había dado a las gallinas, ¿Quizá me confundí...?");
        }
        else
        {
            base.Talk(); // Ejecuta el diálogo configurado en NPC

            if (FarmQuestManager.Instance != null)
            {
                FarmQuestManager.Instance.talkToFarmer = true;
            }
        }
    }
}

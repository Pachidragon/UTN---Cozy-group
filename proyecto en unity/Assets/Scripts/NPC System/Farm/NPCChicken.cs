using UnityEngine;
// Controla la interacción con las gallinas y registra su interrogación como parte de la misión.

public class NPCChicken : NPCBase
{
    [SerializeField] private float distanciaParaHablar = 3f;
    private Transform playerTransform;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
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

    private bool alreadyTalked = false; // Indica si esta gallina ya fue interrogada.

    public override void Talk() // Muestra el diálogo de la gallina y registra su interrogación en la misión.
    {
        if (FarmQuestManager.Instance != null && !FarmQuestManager.Instance.talkToFarmer)
        {
            Debug.Log("[" + nameNPC + "]: ¿Quién eres tú? Habla primero con la granjera.");
            return;
        }

        base.Talk();

        if (!alreadyTalked && FarmQuestManager.Instance != null) // Comprueba que esta gallina no haya sido interrogada anteriormente.
        {
            alreadyTalked = true; // Marca a la gallina como interrogada.
            FarmQuestManager.Instance.chickensInterrogated++;
            Debug.Log("[Misión]: Has interrogado a " + FarmQuestManager.Instance.chickensInterrogated + " gallinas."); // Informa el progreso de la misión.
        }
    }
}

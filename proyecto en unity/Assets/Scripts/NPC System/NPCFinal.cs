using UnityEngine;
// Controla la interacción con el NPC que desencadena la victoria del juego (Mirtha)

public class NPCVictoria : NPCBase
{
    [SerializeField] private GameObject prefabItemFinal;
    [SerializeField] private Transform pivotSpawnItemFinal;
    [SerializeField] private float distanceToTalk = 4f;
    private Transform playerTransform;
    private bool win = false; // Indica si la victoria ya fue otorgada.

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }
    void Update()
    {
        if (playerTransform == null || win) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanceToTalk)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }
    public override void Talk() // Ejecuta el diálogo del NPC 
    {
        if (win) return;

        base.Talk();
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
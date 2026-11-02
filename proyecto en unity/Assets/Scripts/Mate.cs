using UnityEngine;

public class MateObject : MonoBehaviour, IInteractable
{
    [Header("Configuración del Final")]
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private ItemData yerbaMateData; 

    private Transform playerTransform;
    private bool juegoTerminado = false;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update()
    {
        if (playerTransform == null || juegoTerminado) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanciaInteraccion)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }

    public void Interact()
    {
        if (juegoTerminado || InventoryManager.Instance == null || yerbaMateData == null) return;

        // Pregunta si la yerba está guardada en los slots
        if (!InventoryManager.Instance.HasItem(yerbaMateData))
        {
            Debug.Log("[Mate]: No puedes interactuar, te falta la Yerba Mate en tu inventario.");
            return; // Bloquea la interacción si no la tienes
        }

        // Tiene la yerba mate en la mano
        juegoTerminado = true;
        Debug.Log("[Mate]: ¡Yerba Mate colocada con éxito en el recipiente ceremonial!");

        // 1. Consume 1 unidad de yerba del inventario
        InventoryManager.Instance.RemoveItem(yerbaMateData, 1);

        // 2. Cierra la interfaz de diálogos si quedó colgada
        if (Dialogues.Instance != null && Dialogues.Instance.IsDialogueActive)
        {
            Dialogues.Instance.CloseDialogue();
        }

        // 3. Detona la pantalla de victoria pausando el juego
        if (GameCode.Instance != null)
        {
            GameCode.Instance.VictoryOn();
        }
    }
}


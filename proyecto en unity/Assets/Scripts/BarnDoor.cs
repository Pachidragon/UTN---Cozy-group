using Unity.VisualScripting;
using UnityEngine;
// Controla la apertura de la puerta del establo según la proximidad del jugador y el progreso de la misión.
public class BarnDoor : MonoBehaviour
{
    [SerializeField] private float distanceToOpen = 3f; // Distancia máxima a la que el jugador puede interactuar con la puerta.

    private Transform playerTransform;
    private bool openDoor = false;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player"); // Busca el objeto del jugador en la escena.
        if (playerObj != null) playerTransform = playerObj.transform; // Guarda la referencia a su Transform.
    }

    void Update()
    {
        if (playerTransform == null || openDoor) return; // No continúa si no encuentra al jugador o la puerta ya está abierta.

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanceToOpen)
        {
            if (distancia <= distanceToOpen)
            {
                if (FarmQuestManager.Instance != null && FarmQuestManager.Instance.chickensInterrogated >= 2) // Comprueba si se interrogó a las dos gallinas necesarias.
                {
                    OpenDoor();
                }
                else
                {
                    if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        Debug.Log("[Sistema]: La puerta del establo está cerrada con llave. Las gallinas te miran de reojo de forma sospechosa...");
                    }
                }
            }
            else
            {
                if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("[Sistema]: Puerta cerrada. Deberías investigar más en la granja antes de entrar aquí.");
                }
            }
        }
    }

    void OpenDoor() // Abre la puerta y la desactiva de la escena.
    {
        openDoor = true; // Marca la puerta como abierta para evitar nuevas interacciones.
        Debug.Log("[Sistema]: Puerta abierta");
        gameObject.SetActive(false); // Desactiva el objeto de la puerta.
    }
}

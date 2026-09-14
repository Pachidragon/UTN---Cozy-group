using UnityEngine;

public class BarnDoor : MonoBehaviour
{
    [SerializeField] private float distanceToOpen = 3f;
    private Transform playerTransform;
    private bool openDoor = false;

    void Start()
    {
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update()
    {
        if (playerTransform == null || openDoor) return;

        float distancia = Vector3.Distance(transform.position, playerTransform.position);

        if (distancia <= distanceToOpen)
        {
            if (distancia <= distanceToOpen)
            {
                if (FarmQuestManager.Instance != null && FarmQuestManager.Instance.chickensInterrogated >= 2)
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

    void OpenDoor()
    {
        openDoor = true;
        Debug.Log("[Sistema]: Puerta abierta");
        gameObject.SetActive(false);
    }
}

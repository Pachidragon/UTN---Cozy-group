using UnityEngine;

public class Recolectable : MonoBehaviour, IInteractable
{
    [SerializeField] private float distanciaParaRecoger = 2.5f;
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
        if (distancia <= distanciaParaRecoger)
        {
            if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }
    }

    public void Interact()
    {
        Debug.Log("¡Recolectado!");
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Recolectable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool wrongItem = false;

    public void Interact()
    {
        Debug.Log("Recolectado");

        if (wrongItem && GameCode.Instance != null)
        {
            GameCode.Instance.LoseOn();
        }
        Destroy(gameObject);
    }
}

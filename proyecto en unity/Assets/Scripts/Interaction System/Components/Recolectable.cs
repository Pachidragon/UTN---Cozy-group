using UnityEngine;

public class Recolectable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Recolectado");
        Destroy(gameObject);
    }
}

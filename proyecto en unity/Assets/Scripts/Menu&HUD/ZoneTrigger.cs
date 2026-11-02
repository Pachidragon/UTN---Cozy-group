using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] private QuestTrackerUI.GameZone zonaAActivar;

    private void OnTriggerEnter(Collider other)
    {
        // Cuando Clementina cruza el umbral invisible, el panel cambia de página
        if (other.CompareTag("Player") && QuestTrackerUI.Instance != null)
        {
            QuestTrackerUI.Instance.ChangeZone(zonaAActivar);
            Debug.Log($"[Misiones]: Cambiando panel visual a {zonaAActivar}");
        }
    }
}

using UnityEngine;

// Trigger de música. Se pone en un objeto con un Collider (Is Trigger) en puertas, portales o zonas.
// El jugador tiene que tener el tag configurado abajo (por defecto "Player").

[RequireComponent(typeof(Collider))]
public class MusicTrigger : MonoBehaviour
{
    public enum Accion { Nothing, EnterLevel, EnterZone, LeaveZone }

    [SerializeField] private Accion onEnter = Accion.Nothing;
    [SerializeField] private Accion onExit = Accion.Nothing;

    [Tooltip("Solo para EnterLevel.")]
    [SerializeField] private LevelMusic nivel;

    [Tooltip("Índice de la zona en el LevelMusic del nivel actual (0 = A, 1 = B, 2 = C, 3 = D).")]
    [SerializeField, Range(0, 3)] private int zona;

    [SerializeField] private string playerTag = "Player";

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag)) Ejecutar(onEnter);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag)) Ejecutar(onExit);
    }

    private void Ejecutar(Accion accion)
    {
        MusicManager music = MusicManager.Instance;
        if (music == null) return;

        switch (accion)
        {
            case Accion.EnterLevel: music.EnterLevel(nivel); break;
            case Accion.EnterZone: music.EnterZone(zona); break;
            case Accion.LeaveZone: music.ExitZone(zona); break;
        }
    }
}

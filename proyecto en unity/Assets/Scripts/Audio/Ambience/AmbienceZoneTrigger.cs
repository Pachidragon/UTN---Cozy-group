using UnityEngine;
using UnityEngine.Audio;

public class ZonaAmbiente : MonoBehaviour
{
    [Header("Configuración del Mixer")]
    public AudioMixerSnapshot snapshotAmbiente;
    public float tiempoTransicion = 2.0f;

    [Header("Configuración del Tag")]
    public string tagJugador = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // Solo cambia el audio si el que entra es el Jugador principal
        if (other.CompareTag(tagJugador) && snapshotAmbiente != null)
        {
            snapshotAmbiente.TransitionTo(tiempoTransicion);
            Debug.Log("Transición exitosa al snapshot: " + snapshotAmbiente.name);
        }
    }
}

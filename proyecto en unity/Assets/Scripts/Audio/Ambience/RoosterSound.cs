using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class RoosterSound : MonoBehaviour
{
    [Header("Referencias de Mezcla (Bloque D)")]
    [SerializeField] private AudioMixerSnapshot snapshotRooster; // Snapshot donde el fader del Gallo sube a 0dB

    private AudioSource roosterItem;
    private bool roosterAwake = false;

    private void Awake()
    {
        roosterItem = GetComponent<AudioSource>();

        // El AudioSource arranca apagado para que no consuma recursos
        roosterItem.playOnAwake = false;
        roosterItem.enabled = false;
    }

    private void Update()
    {
        if (roosterAwake) return;

        // Revisa si el Player ya terminó la charla con el caballo
        if (FarmQuestManager.Instance != null && FarmQuestManager.Instance.talkToHorse)
        {
            SetRoosterOn();
        }
    }

    private void SetRoosterOn()
    {
        roosterAwake = true;

        // Enciende el componente en el techo y da Play
        roosterItem.enabled = true;
        roosterItem.Play();

        // El Mixer sube su fader de forma suave en 1.5 segundos
        if (snapshotRooster != null)
        {
            snapshotRooster.TransitionTo(1.5f);
        }

        Debug.Log("[Audio Bloque D]: ¡El Caballo confesó el secreto! El Gallo ha despertado en el techo y empieza a cantar.");
    }
}

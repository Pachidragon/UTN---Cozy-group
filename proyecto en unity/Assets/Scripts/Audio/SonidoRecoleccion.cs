using UnityEngine;
using UnityEngine.Audio;

public class SonidoRecoleccion : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip sonido;
    [SerializeField] private AudioMixerGroup grupoSFX;

    private bool cerrandoEscena = false;

    private void OnApplicationQuit()
    {
        cerrandoEscena = true;
    }

    private void OnDestroy()
    {
        if (cerrandoEscena || !gameObject.scene.isLoaded || sonido == null)
            return;

        GameObject audioTemporal = new GameObject("SFX_Recoleccion");

        AudioSource fuente = audioTemporal.AddComponent<AudioSource>();

        fuente.outputAudioMixerGroup = grupoSFX;
        fuente.playOnAwake = false;
        fuente.spatialBlend = 0f;

        fuente.PlayOneShot(sonido);

        Destroy(audioTemporal, sonido.length + 0.1f);
    }
}
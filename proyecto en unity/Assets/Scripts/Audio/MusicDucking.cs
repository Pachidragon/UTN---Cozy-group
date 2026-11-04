using UnityEngine;
using UnityEngine.Audio;

public class MusicDucking : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource narrador;

    public float volumenBajo = -15f;
    public float velocidad = 30f;

    private float volumenActual = 0f;

    private void Update()
    {
        if (mixer == null || narrador == null)
            return;

        float objetivo = narrador.isPlaying ? volumenBajo : 0f;

        volumenActual = Mathf.MoveTowards(
            volumenActual,
            objetivo,
            velocidad * Time.unscaledDeltaTime
        );

        mixer.SetFloat("NarradorDuck", volumenActual);
    }

    private void OnDisable()
    {
        if (mixer != null)
            mixer.SetFloat("NarradorDuck", 0f);
    }
}
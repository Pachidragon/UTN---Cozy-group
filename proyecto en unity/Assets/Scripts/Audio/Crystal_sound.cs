using UnityEngine;

public class SonidoProximidad : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clipsCristal;

    [Header("Configuración de Distancia")]
    [SerializeField] private float distanciaActivacion = 6f;

    [Header("Configuración de Reproducción (Anti-Cansancio)")]
    [SerializeField] private float tiempoMinimoEntreSonidos = 2f;
    [SerializeField] private float tiempoMaximoEntreSonidos = 5f;

    private Transform jugador;
    private bool estaCerca = false;
    private float temporizador = 0f;

    void Start()
    {
        // Buscamos al jugador por su Tag al iniciar
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        else
        {
            Debug.LogWarning($"[SonidoProximidad] No se encontró al jugador en {gameObject.name}. Revisá el Tag 'Player'.");
        }

        // Configuramos el AudioSource
        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.spatialBlend = 1f; // Forzamos 3D
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (jugador == null || audioSource == null || clipsCristal.Length == 0) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);
        bool estaCercaAhora = distancia <= distanciaActivacion;

        // Detectar entrada al radio
        if (estaCercaAhora && !estaCerca)
        {
            estaCerca = true;
            temporizador = 0f; // Forzamos sonido inmediato al entrar
        }
        // Detectar salida del radio
        else if (!estaCercaAhora && estaCerca)
        {
            estaCerca = false;
            ReiniciarTemporizador(); // Para que no suene apenas vuelva
        }

        // Reproducción temporizada
        if (estaCercaAhora)
        {
            temporizador -= Time.deltaTime;

            if (temporizador <= 0f)
            {
                ReproducirSonidoAleatorio();
                ReiniciarTemporizador();
            }
        }
    }

    private void ReproducirSonidoAleatorio()
    {
        int indiceAleatorio = Random.Range(0, clipsCristal.Length);
        AudioClip clipElegido = clipsCristal[indiceAleatorio];

        if (clipElegido != null)
        {
            audioSource.PlayOneShot(clipElegido);
        }
    }

    private void ReiniciarTemporizador()
    {
        temporizador = Random.Range(tiempoMinimoEntreSonidos, tiempoMaximoEntreSonidos);
    }
}
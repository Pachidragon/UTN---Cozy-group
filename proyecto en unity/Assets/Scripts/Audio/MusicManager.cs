using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// Controla toda la música. Cada capa tiene su AudioSource y su grupo en el Audio Mixer, cambios con snapshots (TransitionTo) y transiciones suaves.
// Va en un objeto raíz de la escena del menú. Sobrevive a los cambios de escena.
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public LevelMusic music;
    }

    [Header("Grupos del Audio Mixer (hijos del grupo Música)")]
    [SerializeField] private AudioMixerGroup groupBase;
    [SerializeField] private AudioMixerGroup groupRitmo;
    [SerializeField] private AudioMixerGroup[] groupZonas = new AudioMixerGroup[LevelMusic.MaxZones];
    [SerializeField] private AudioMixerGroup groupFinal;

    [Header("Snapshots")]
    [SerializeField] private AudioMixerSnapshot snapSilencio;
    [SerializeField] private AudioMixerSnapshot snapBase;
    [SerializeField] private AudioMixerSnapshot snapBaseRitmo;
    [SerializeField] private AudioMixerSnapshot[] snapZonas = new AudioMixerSnapshot[LevelMusic.MaxZones];
    [SerializeField] private AudioMixerSnapshot snapFinal;

    [Header("Duración de las transiciones")]
    [SerializeField] private float fadeOut = 1f;     // Al cambiar de nivel o escena, baja a silencio.
    [SerializeField] private float fadeIn = 1.5f;    // Sube la música del nivel nuevo.
    [SerializeField] private float layerFade = 1f;   // Capa rítmica.
    [SerializeField] private float zoneFade = 2f;    // Entrar y salir de zonas.
    [SerializeField] private float finalFade = 0.5f; // Victoria y derrota.

    [Header("Victoria y derrota")]
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private AudioClip defeatClip;

    [Header("Música por escena")]
    [SerializeField] private SceneMusic[] sceneMusics;

    private AudioSource baseSrc;
    private AudioSource ritmoSrc;
    private AudioSource finalSrc;
    private readonly AudioSource[] zoneSrc = new AudioSource[LevelMusic.MaxZones];
    private readonly List<AudioSource> verticalSources = new List<AudioSource>(); // Las que suenan sincronizadas.
    private readonly List<AudioSource> allSources = new List<AudioSource>();

    private LevelMusic currentLevel;
    private int currentZone = -1;     // -1 = no está en ninguna zona.
    private bool rhythmOn;
    private bool verticalPlaying;
    private bool finished;            // Ya sonó la música de victoria o derrota.
    private bool hasMusic;
    private int lastSceneHandle = -1;
    private Coroutine levelRoutine;
    private Coroutine cleanupRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        baseSrc = CreateSource(groupBase, true);
        ritmoSrc = CreateSource(groupRitmo, true);
        for (int i = 0; i < zoneSrc.Length; i++)
        {
            AudioMixerGroup g = i < groupZonas.Length ? groupZonas[i] : null;
            zoneSrc[i] = CreateSource(g, true);
        }
        finalSrc = CreateSource(groupFinal, false);

        if (snapSilencio != null) snapSilencio.TransitionTo(0f); // Arranca en silencio.

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (Instance != this) return;
        ApplyScene(SceneManager.GetActiveScene()); // Por si se arranca directo en esa escena.
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance = null;
    }

    private AudioSource CreateSource(AudioMixerGroup group, bool loop)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.playOnAwake = false;
        a.loop = loop;
        a.spatialBlend = 0f; // 2D
        a.outputAudioMixerGroup = group;
        allSources.Add(a);
        return a;
    }

    // ---------- Escenas ----------

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyScene(scene);
    }

    private void ApplyScene(Scene scene)
    {
        if (scene.handle == lastSceneHandle) return; // Evita procesar dos veces la misma escena.
        lastSceneHandle = scene.handle;

        foreach (SceneMusic entry in sceneMusics)
        {
            if (entry.sceneName == scene.name)
            {
                SetLevel(entry.music, true); // Forzado, al recargar la escena la música reinicia.
                return;
            }
        }
    }

    // ---------- API pública ----------

    // Cambia a la música de otro nivel (la llama un MusicTrigger al cruzar una puerta o portal).
    public void EnterLevel(LevelMusic level)
    {
        SetLevel(level, false);
    }

    // Sube o baja la capa rítmica (la llama el CraftingPanel al abrir y cerrar).
    public void SetRhythm(bool on)
    {
        if (rhythmOn == on) return;
        rhythmOn = on;
        Refresh(layerFade);
    }

    public void EnterZone(int index)
    {
        if (currentLevel == null || !currentLevel.HasZone(index)) return;
        currentZone = index;
        Refresh(zoneFade);
    }

    // Solo sale si todavía está en esa zona (evita pisar otra zona a la que ya entró).
    public void ExitZone(int index)
    {
        if (currentZone != index) return;
        currentZone = -1;
        Refresh(zoneFade);
    }

    public void PlayVictory() { PlayFinal(victoryClip); }
    public void PlayDefeat() { PlayFinal(defeatClip); }

    // ---------- Lógica interna ----------

    private void SetLevel(LevelMusic level, bool force)
    {
        if (level == null) return;
        if (!force && level == currentLevel && !finished) return;

        currentLevel = level;
        finished = false;
        rhythmOn = false;
        currentZone = -1;

        if (levelRoutine != null) StopCoroutine(levelRoutine);
        if (cleanupRoutine != null) { StopCoroutine(cleanupRoutine); cleanupRoutine = null; }
        levelRoutine = StartCoroutine(LevelRoutine(level));
    }

    // Fade out de lo que suena, cambia los clips y hace fade in de la base del nivel nuevo.
    private IEnumerator LevelRoutine(LevelMusic level)
    {
        yield return null; // Espera un frame para que levelRoutine quede bien asignada.

        if (hasMusic)
        {
            snapSilencio.TransitionTo(fadeOut);
            yield return new WaitForSecondsRealtime(fadeOut);
        }

        StopAll();
        AssignClips(level);
        EnsureVertical();
        hasMusic = true;

        levelRoutine = null;
        Refresh(fadeIn);
    }

    private void AssignClips(LevelMusic level)
    {
        baseSrc.clip = level.baseClip;
        ritmoSrc.clip = level.ritmoClip;
        for (int i = 0; i < zoneSrc.Length; i++)
            zoneSrc[i].clip = level.HasZone(i) ? level.zonas[i].clip : null;

        verticalSources.Clear();
        if (level.baseClip != null) verticalSources.Add(baseSrc);
        if (level.ritmoClip != null) verticalSources.Add(ritmoSrc);
        for (int i = 0; i < zoneSrc.Length; i++)
            if (level.IsVertical(i)) verticalSources.Add(zoneSrc[i]);
    }

    // Elige el snapshot que corresponde al estado actual y hace la transición.
    private void Refresh(float time)
    {
        if (finished || levelRoutine != null || currentLevel == null) return;
        if (cleanupRoutine != null) StopCoroutine(cleanupRoutine);

        AudioMixerSnapshot target;
        if (currentZone >= 0)
        {
            if (!currentLevel.IsVertical(currentZone))
            {
                // Horizontal arranca desde el principio si todavía no suena.
                if (!zoneSrc[currentZone].isPlaying) zoneSrc[currentZone].Play();
            }
            else
            {
                EnsureVertical();
            }
            target = currentZone < snapZonas.Length ? snapZonas[currentZone] : snapBase;
        }
        else
        {
            EnsureVertical(); // Si se venía de una zona horizontal, la base reinicia desde el principio.
            target = rhythmOn ? snapBaseRitmo : snapBase;
        }

        if (target != null) target.TransitionTo(time);
        cleanupRoutine = StartCoroutine(Cleanup(time));
    }

    // Cuando termina el fade, detiene lo que quedó en silencio y ya no hace falta.
    private IEnumerator Cleanup(float time)
    {
        yield return new WaitForSecondsRealtime(time + 0.1f);

        for (int i = 0; i < zoneSrc.Length; i++)
        {
            if (!currentLevel.IsVertical(i) && i != currentZone) zoneSrc[i].Stop();
        }

        // En una zona horizontal la base y las capas verticales se paran del todo.
        if (currentZone >= 0 && !currentLevel.IsVertical(currentZone)) StopVertical();

        cleanupRoutine = null;
    }

    // Arranca todas las capas verticales juntas, en el mismo instante del reloj de audio.
    private void EnsureVertical()
    {
        if (verticalPlaying) return;

        double start = AudioSettings.dspTime + 0.2;
        foreach (AudioSource s in verticalSources) s.PlayScheduled(start);
        verticalPlaying = true;
    }

    private void StopVertical()
    {
        foreach (AudioSource s in verticalSources) s.Stop();
        verticalPlaying = false;
    }

    private void StopAll()
    {
        foreach (AudioSource s in allSources) s.Stop();
        verticalPlaying = false;
    }

    private void PlayFinal(AudioClip clip)
    {
        if (finished || clip == null) return;
        finished = true;

        if (levelRoutine != null) { StopCoroutine(levelRoutine); levelRoutine = null; }
        if (cleanupRoutine != null) { StopCoroutine(cleanupRoutine); cleanupRoutine = null; }

        finalSrc.clip = clip;
        finalSrc.Play();
        if (snapFinal != null) snapFinal.TransitionTo(finalFade);
    }
}

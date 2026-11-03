using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelMusic", menuName = "Scriptable Objects/LevelMusic")]
public class LevelMusic : ScriptableObject
{
    public const int MaxZones = 4; // Zonas del mixer.

    [Serializable]
    public class ZoneTrack
    {
        public string zoneName;
        public AudioClip clip;

        [Tooltip("Vertical: suena desde el inicio del nivel con el volumen en 0 y solo se sube." +
                 "Horizontal: arranca recién al entrar a la zona y la base se detiene.")]
        public bool vertical = true;
    }

    [Header("Capas del nivel")]
    public AudioClip baseClip;   // Música base del nivel.
    public AudioClip ritmoClip;  // Capa rítmica.

    [Header("Zonas (máximo 4)")]
    public ZoneTrack[] zonas = new ZoneTrack[0];

    public bool HasZone(int i)
    {
        return i >= 0 && i < zonas.Length && i < MaxZones && zonas[i] != null && zonas[i].clip != null;
    }

    public bool IsVertical(int i)
    {
        return HasZone(i) && zonas[i].vertical;
    }
}

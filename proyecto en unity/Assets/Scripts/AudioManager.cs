using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource musica;
    public AudioSource narrador;

    public float volumenNormal = 1f;
    public float volumenBajo = 0.2f;
    public float velocidad = 2f;

    private void Update()
    {
        if (narrador.isPlaying)
        {
            musica.volume = Mathf.MoveTowards(
                musica.volume,
                volumenBajo,
                velocidad * Time.deltaTime
            );
        }
        else
        {
            musica.volume = Mathf.MoveTowards(
                musica.volume,
                volumenNormal,
                velocidad * Time.deltaTime
            );
        }
    }
}
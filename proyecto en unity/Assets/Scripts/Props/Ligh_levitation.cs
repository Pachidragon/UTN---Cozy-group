using UnityEngine;

[ExecuteAlways]
public class Levitation : MonoBehaviour
{
    [Header("Configuración de Flotación")]
    [SerializeField, Range(0f, 2f)]
    private float amplitude = 0.5f;

    [SerializeField, Range(0.1f, 5f)]
    private float speed = 2f;

    [Header("Herramientas de Editor")]
    [Tooltip("Tildá esto para previsualizar el movimiento en el editor.")]
    [SerializeField] private bool previewInEditor = false;

    private Vector3 initialPosition;
    private float randomTimeOffset;

    void Start()
    {
        initialPosition = transform.position;
        randomTimeOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        // Si estamos en el editor y el preview está apagado, no hacemos nada.
        if (!Application.isPlaying && !previewInEditor) return;

        // Calculamos el movimiento
        float offsetY = Mathf.Sin(Time.time * speed + randomTimeOffset) * amplitude;

        // Aplicamos el movimiento al eje Y manteniendo la posición base
        transform.position = initialPosition + new Vector3(0, offsetY, 0);
    }
}
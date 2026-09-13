using Unity.VisualScripting;
using UnityEngine;

public class platformMovement : MonoBehaviour
{
    [SerializeField] Vector3 direccionMovimiento = Vector3.up;

    [SerializeField] float distancia;

    [SerializeField] float velocidad;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float factorMovimiento = Mathf.Sin(Time.time * velocidad);

        transform.position = posicionInicial + (direccionMovimiento.normalized * factorMovimiento * (distancia / 2f));
    }
}

using UnityEngine;
using System.Collections.Generic;

public class jumpPad : MonoBehaviour
{
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float gravedadSimulada = 30f;


    private class DatosSaltoJugador
    {
        public CharacterController controlador;
        public float velocidadVertical;
    }

    private List<DatosSaltoJugador> jugadoresSaltando = new List<DatosSaltoJugador>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {

                DatosSaltoJugador datos = jugadoresSaltando.Find(j => j.controlador == cc);

                if (datos == null)
                {
                    datos = new DatosSaltoJugador { controlador = cc };
                    jugadoresSaltando.Add(datos);
                }


                datos.velocidadVertical = fuerzaSalto;
            }
        }
    }

    void Update()
    {
        for (int i = jugadoresSaltando.Count - 1; i >= 0; i--)
        {
            DatosSaltoJugador jugador = jugadoresSaltando[i];

            if (jugador.controlador == null || !jugador.controlador.gameObject.activeInHierarchy)
            {
                jugadoresSaltando.RemoveAt(i);
                continue;
            }

            jugador.velocidadVertical -= gravedadSimulada * Time.deltaTime;

            Vector3 vectorImpulso = new Vector3(0f, jugador.velocidadVertical, 0f) * Time.deltaTime;
            jugador.controlador.Move(vectorImpulso);

            if (jugador.controlador.isGrounded && jugador.velocidadVertical <= 0)
            {
                jugadoresSaltando.RemoveAt(i);
                
            }
        }
    }
}
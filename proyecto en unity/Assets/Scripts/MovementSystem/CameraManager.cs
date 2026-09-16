using System;
using UnityEngine;
using UnityEngine.InputSystem;
// Controla la rotación y posición de la cámara siguiendo al jugador y evitando atravesar objetos.

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target; // Referencia al jugador que sigue la cámara.
    [SerializeField] private float distanceToPlayer; // Distancia de la cámara respecto al jugador.

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius;
    [SerializeField] private float collisionOffset;

    private Vector2 input; // Almacena el movimiento del mouse.

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;
    private CameraRotation cameraRotation;

    public void Look(InputAction.CallbackContext context) // Recibe y almacena el movimiento del mouse.
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update() // Actualiza la rotación de la cámara según el movimiento del mouse.
    {
        cameraRotation.Yaw += input.x * mouseSensitivity.horizontal * Time.deltaTime;
        cameraRotation.Pitch += input.y * mouseSensitivity.vertical * Time.deltaTime;
        cameraRotation.Pitch = Mathf.Clamp(cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
    }

    private void LateUpdate() // Actualiza la posición y rotación de la cámara después del movimiento del jugador.
    {
        transform.eulerAngles = new Vector3(cameraRotation.Pitch, cameraRotation.Yaw, 0.0f);

        Vector3 desiredPosition = target.position - transform.forward * distanceToPlayer;
        transform.position = GetCollisionAdjustedPosition(desiredPosition);
    }

    private Vector3 GetCollisionAdjustedPosition(Vector3 desiredPosition) // Calcula la posición final de la cámara teniendo en cuenta las colisiones.
    {
        Vector3 direction = desiredPosition - target.position;
        float maxDistance = direction.magnitude;
        direction.Normalize();

        if(Physics.SphereCast(target.position, collisionRadius, direction, out RaycastHit hit, maxDistance, collisionMask))
        {
            return target.position + direction * (hit.distance - collisionOffset);
        }
        return desiredPosition;
    }
}
// Define la sensibilidad horizontal y vertical del movimiento del mouse.
[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CameraRotation // Almacena los ángulos actuales de rotación de la cámara.
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle // Define los límites mínimo y máximo de rotación vertical de la cámara.
{
    public float min;
    public float max;
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float distanceToPlayer;

    [Header("Collision")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius;
    [SerializeField] private float collisionOffset;

    private Vector2 input;

    [SerializeField] private MouseSensitivity mouseSensitivity;
    [SerializeField] private CameraAngle cameraAngle;
    private CameraRotation cameraRotation;

    private void Awake() => distanceToPlayer = Vector3.Distance(transform.position, target.position);

    public void Look(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        cameraRotation.Yaw += input.x * mouseSensitivity.horizontal * Time.deltaTime;
        cameraRotation.Pitch += input.y * mouseSensitivity.vertical * Time.deltaTime;
        cameraRotation.Pitch = Mathf.Clamp(cameraRotation.Pitch, cameraAngle.min, cameraAngle.max);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(cameraRotation.Pitch, cameraRotation.Yaw, 0.0f);

        Vector3 desiredPosition = target.position - transform.forward * distanceToPlayer;
        transform.position = GetCollisionAdjustedPosition(desiredPosition);
    }

    private Vector3 GetCollisionAdjustedPosition(Vector3 desiredPosition)
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

[Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CameraRotation
{
    public float Pitch;
    public float Yaw;
}

[Serializable]
public struct CameraAngle
{
    public float min;
    public float max;
}

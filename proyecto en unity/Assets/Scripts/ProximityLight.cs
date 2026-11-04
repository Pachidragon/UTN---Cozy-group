using UnityEngine;

public class ProximityLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Light mushroomLight;

    [Header("Settings")]
    [SerializeField] private float activationDistance = 3f;
    [SerializeField] private float maxIntensity = 2f;
    [SerializeField] private float fadeSpeed = 3f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (mushroomLight != null)
        {
            mushroomLight.intensity = 0f;
            mushroomLight.enabled = true;
        }
    }

    private void Update()
    {
        if (player == null || mushroomLight == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        float targetIntensity =
            distance <= activationDistance ? maxIntensity : 0f;

        mushroomLight.intensity = Mathf.MoveTowards(
            mushroomLight.intensity,
            targetIntensity,
            fadeSpeed * Time.deltaTime
        );
    }
}
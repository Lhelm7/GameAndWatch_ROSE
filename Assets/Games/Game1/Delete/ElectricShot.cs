using UnityEngine;

public class ElectricPulse : MonoBehaviour
{
    [Header("Movement")]
    public Transform startPoint;
    public Transform endPoint;
    public float travelDuration = 1f;

    [Header("Visuals")]
    public Light pointLight;
    public float maxLightIntensity = 2f;

    float timer = 0f;

    void Start()
    {
        transform.position = startPoint.position;

        if (pointLight != null)
            pointLight.intensity = maxLightIntensity;
    }

    void Update()
    {
        timer += Time.deltaTime / travelDuration;

        transform.position = Vector3.Lerp(
            startPoint.position,
            endPoint.position,
            timer
        );

        // Diminution légère de la lumière vers la fin
        if (pointLight != null)
            pointLight.intensity = Mathf.Lerp(maxLightIntensity, 0f, timer);

        if (timer >= 1f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mort du joueur
            Debug.Log("Player electrocuté !");
            // Appelle ici ton système de mort
        }
    }
}
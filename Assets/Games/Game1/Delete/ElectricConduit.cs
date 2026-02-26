using System.Collections;
using UnityEngine;

public class ElectricConduit : MonoBehaviour
{
    [Header("References")]
    public Transform startPoint;
    public Transform endPoint;
    public GameObject electricPulsePrefab;

    [Header("Timing")]
    public float delayBetweenPulses = 2f;
    public float pulseDuration = 1f;

    [Header("State")]
    public bool isActive = true;

    void Start()
    {
        StartCoroutine(PulseLoop());
    }

    IEnumerator PulseLoop()
    {
        while (isActive)
        {
            SpawnPulse();
            yield return new WaitForSeconds(delayBetweenPulses);
        }
    }

    void SpawnPulse()
    {
        GameObject pulse = Instantiate(
            electricPulsePrefab,
            startPoint.position,
            Quaternion.identity
        );

        ElectricPulse pulseScript = pulse.GetComponent<ElectricPulse>();
        pulseScript.startPoint = startPoint;
        pulseScript.endPoint = endPoint;
        pulseScript.travelDuration = pulseDuration;
    }

    // Appelé par un générateur par exemple
    public void Deactivate()
    {
        isActive = false;
    }

    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(PulseLoop());
        }
    }
}
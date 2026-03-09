using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("UI")]
    public TextMeshProUGUI liveScoretText;

    [Header("Score Settings")]
    [Tooltip("Multiplicateur : unités Unity → mètres affichés.")]
    public float metersPerUnit = 2.5f;

    public int CurrentMeters { get; private set; }

    private float startY;
    private float highestY;

    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        if (player != null)
        {
            startY   = player.position.y;
            highestY = startY;
        }
    }

    void Update()
    {
        if (player == null) return;

        // On ne compte que la montée, jamais la descente
        if (player.position.y > highestY)
            highestY = player.position.y;

        CurrentMeters = Mathf.Max(0, Mathf.FloorToInt((highestY - startY) * metersPerUnit));

        if (liveScoretText != null)
            liveScoretText.text = CurrentMeters + " m";
    }
}
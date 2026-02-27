using UnityEngine;
using System.Collections.Generic;

public class ParasiteSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<ParasiteData> parasiteTypes;
    public GameObject parasitePrefab;

    private ParasiteGameManager gameManager;

    /// <summary>Spawn les parasites de la vague selon la séquence et le parasiteCount.</summary>
    public void StartSpawning(WaveData wave)
    {
        gameManager = FindFirstObjectByType<ParasiteGameManager>();
        SpawnAllParasites(wave);
    }

    void SpawnAllParasites(WaveData wave)
    {
        List<ColorType> sequence = gameManager.GetAllowedColors();

        List<Transform> shuffled = new List<Transform>(spawnPoints);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        // Spawn au moins autant que la séquence, jusqu'à parasiteCount
        int count = Mathf.Clamp(wave.parasiteCount, sequence.Count, shuffled.Count);

        for (int i = 0; i < count; i++)
        {
            // Cycle sur les couleurs de la séquence si parasiteCount > sequenceLength
            ColorType color = sequence[i % sequence.Count];
            ParasiteData dataToSpawn = parasiteTypes.Find(p => p.colorType == color);

            if (dataToSpawn == null)
            {
                Debug.LogWarning($"[Spawner] Aucun ParasiteData pour {color}");
                continue;
            }

            GameObject obj = Instantiate(parasitePrefab, shuffled[i].position, Quaternion.identity);
            obj.GetComponent<Parasite>().Init(dataToSpawn, gameManager);
        }

        Debug.Log($"[Spawner] {count} parasite(s) spawné(s) — séquence: {string.Join(", ", sequence)}");
    }
}
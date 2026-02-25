using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParasiteSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public List<ParasiteData> parasiteTypes;
    public GameObject parasitePrefab;

    private ParasiteGameManager gameManager;

    public void StartSpawning(WaveData wave)
    {
        gameManager = FindObjectOfType<ParasiteGameManager>();
        StartCoroutine(SpawnRoutine(wave));
    }

    IEnumerator SpawnRoutine(WaveData wave)
    {
        for (int i = 0; i < wave.parasiteCount; i++)
        {
            SpawnRandomParasite();
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
        void SpawnRandomParasite()
{
    List<ColorType> allowedColors = gameManager.GetAllowedColors();

    // Choisit une couleur autorisée
    ColorType randomColor = allowedColors[Random.Range(0, allowedColors.Count)];

    // Trouve le ParasiteData correspondant
    ParasiteData dataToSpawn = parasiteTypes.Find(p => p.colorType == randomColor);

    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

    GameObject obj = Instantiate(parasitePrefab, randomPoint.position, Quaternion.identity);
    obj.GetComponent<Parasite>().Init(dataToSpawn, gameManager);
}
}

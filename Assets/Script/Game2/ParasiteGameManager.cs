using UnityEngine;
using System.Collections.Generic;
public class ParasiteGameManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveData> waves;
    private int currentWaveIndex = 0;

    [Header("Sequence")]
    public List<ColorType> currentSequence = new List<ColorType>();
    private int currentInputIndex = 0;

    [Header("References")]
    public ParasiteSpawner spawner;
    public SequenceDisplayUI sequenceUI;

    private WaveData currentWave;

    void Start()
    {
        StartWave();
    }

    void StartWave()
    {
        currentWave = waves[currentWaveIndex];
        GenerateSequence(currentWave.sequenceLength); // Toujours aléatoire
        sequenceUI.DisplaySequence(currentSequence, OnSequenceDisplayed);
    }

    void OnSequenceDisplayed()
    {
        // Après affichage, on spawn les parasites
        spawner.StartSpawning(currentWave);
        currentInputIndex = 0;
    }

    void GenerateSequence(int length)
    {
        currentSequence.Clear();

        for (int i = 0; i < length; i++)
        {
            ColorType randomColor = (ColorType)Random.Range(0, 4);
            currentSequence.Add(randomColor);
        }
    }

    public void OnParasiteTouched(ColorType touchedColor)
    {
        if (touchedColor == currentSequence[currentInputIndex])
        {
            currentInputIndex++;

            if (currentInputIndex >= currentSequence.Count)
            {
                WaveComplete();
            }
        }
        else
        {
            Debug.Log("Wrong order");
            currentInputIndex = 0;
        }
    }

    void WaveComplete()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Game Complete!");
            return;
        }

        StartWave();
    }
    public List<ColorType> GetAllowedColors()
    {
        return currentSequence;
    }
}
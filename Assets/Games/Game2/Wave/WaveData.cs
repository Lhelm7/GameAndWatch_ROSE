using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "Parasite/Wave Data")]
public class WaveData : ScriptableObject
{
    public int sequenceLength = 2;
    public int parasiteCount = 5;
    public float spawnInterval = 1f;
}
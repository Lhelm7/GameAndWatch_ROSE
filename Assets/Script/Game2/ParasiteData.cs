using UnityEngine;

[CreateAssetMenu(fileName = "NewParasite", menuName = "Parasite/Parasite Data")]
public class ParasiteData : ScriptableObject
{
    public ColorType colorType;
    public Sprite sprite;
    public int scoreValue = 10;
}
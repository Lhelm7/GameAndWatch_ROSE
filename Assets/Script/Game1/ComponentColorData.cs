using UnityEngine;

[CreateAssetMenu(fileName = "NewColor", menuName = "Game/Component Color Data")]
public class ComponentColorData : ScriptableObject
{
    [Header("Identification")]
    public string colorName;
    public int colorID;
    
    [Header("Visuals")]
    public Sprite sprite;              // ← Sprite spécifique pour cette couleur
    public Color displayColor;         // ← Pour l'affichage UI uniquemen
}
using UnityEngine;

public class Platform : MonoBehaviour
{
    public ColorType platformColor;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Assigns a color type to the platform and updates its visual.
    /// </summary>
    public void SetColor(ColorType color)
    {
        platformColor = color;
        spriteRenderer.color = PlayerController.ColorTypeToUnityColor(color);
    }
}
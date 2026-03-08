using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public PlayerController player;

    public float changeInterval = 5f;

    // C# event: fired when the next color is decided, before it's applied
    public event System.Action<ColorType, float> OnNextColorReady;

    // C# event: fired when the color is actually applied
    public event System.Action<ColorType> OnColorApplied;

    public float TimeRemaining => changeInterval - timer;
    public float Progress      => 1f - (timer / changeInterval);
    public ColorType NextColor  => nextColor;

    private float timer;
    private ColorType nextColor;

    private static readonly ColorType[] AvailableColors =
    {
        ColorType.Blue,
        ColorType.Red,
        ColorType.Green,
        ColorType.Yellow
    };

    void Start()
    {
        // Pick the next color right away so the UI can display it from the start
        nextColor = PickDifferentColor(player.currentColor);
        OnNextColorReady?.Invoke(nextColor, changeInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeInterval)
        {
            ApplyColor();
            timer = 0f;
        }
    }

    /// <summary>
    /// Applies the pre-selected next color to the player, then picks the following one.
    /// </summary>
    void ApplyColor()
    {
        player.SetColor(nextColor);
        OnColorApplied?.Invoke(nextColor);

        nextColor = PickDifferentColor(player.currentColor);
        OnNextColorReady?.Invoke(nextColor, changeInterval);
    }

    ColorType PickDifferentColor(ColorType current)
    {
        ColorType picked;
        do
        {
            picked = AvailableColors[Random.Range(0, AvailableColors.Length)];
        }
        while (picked == current);

        return picked;
    }
}
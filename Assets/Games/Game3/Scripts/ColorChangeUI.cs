using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorChangeUI : MonoBehaviour
{
    [Header("References")]
    public ColorManager colorManager;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public Image timerBar;

    [Header("Next Color Preview")]
    public Image nextColorPreview;
    public TextMeshProUGUI nextColorLabel;

    [Header("Flash on Change")]
    public Image flashOverlay;

    private const float FlashDuration = 0.2f;
    private float flashTimer;
    private bool isFlashing;

    void OnEnable()
    {
        colorManager.OnNextColorReady += HandleNextColorReady;
        colorManager.OnColorApplied   += HandleColorApplied;
    }

    void OnDisable()
    {
        colorManager.OnNextColorReady -= HandleNextColorReady;
        colorManager.OnColorApplied   -= HandleColorApplied;
    }

    void Update()
    {
        UpdateTimer();
        UpdateFlash();
    }
    
    void UpdateTimer()
    {
        float remaining = colorManager.TimeRemaining;
        float progress  = colorManager.Progress;

        if (timerText != null)
            timerText.text = Mathf.CeilToInt(remaining).ToString();

        if (timerBar != null)
            timerBar.fillAmount = progress;
    }
    

    void HandleNextColorReady(ColorType next, float interval)
    {
        Color unityColor = PlayerController.ColorTypeToUnityColor(next);

        if (nextColorPreview != null)
            nextColorPreview.color = unityColor;

        if (nextColorLabel != null)
            nextColorLabel.text = next.ToString();
    }

    void HandleColorApplied(ColorType applied)
    {
        TriggerFlash(PlayerController.ColorTypeToUnityColor(applied));
    }
    

    void TriggerFlash(Color color)
    {
        if (flashOverlay == null) return;
        flashOverlay.color = new Color(color.r, color.g, color.b, 0.5f);
        flashTimer  = FlashDuration;
        isFlashing  = true;
    }

    void UpdateFlash()
    {
        if (!isFlashing || flashOverlay == null) return;

        flashTimer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(flashTimer / FlashDuration) * 0.5f;
        Color c = flashOverlay.color;
        flashOverlay.color = new Color(c.r, c.g, c.b, alpha);

        if (flashTimer <= 0f)
            isFlashing = false;
    }
}


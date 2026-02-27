using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SequenceDisplayUI : MonoBehaviour
{
    public GameObject colorPrefab;
    public Transform container;

    [Tooltip("Secondes d'affichage par couleur avant le spawn")]
    public float displayTime = 1.5f;

    private const float ColorSize = 60f;
    private const float ColorSpacing = 80f;
    private List<GameObject> currentPanels = new List<GameObject>();

    /// <summary>Affiche toutes les couleurs de la séquence simultanément puis appelle onComplete.</summary>
    public void DisplaySequence(List<ColorType> sequence, System.Action onComplete)
    {
        StartCoroutine(DisplayRoutine(sequence, onComplete));
    }

    IEnumerator DisplayRoutine(List<ColorType> sequence, System.Action onComplete)
    {
        foreach (var panel in currentPanels)
            Destroy(panel);
        currentPanels.Clear();

        float totalWidth = (sequence.Count - 1) * ColorSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < sequence.Count; i++)
        {
            GameObject panel = Instantiate(colorPrefab, container);
            RectTransform rt = panel.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(ColorSize, ColorSize);
            rt.anchoredPosition = new Vector2(startX + i * ColorSpacing, 0f);
            panel.GetComponent<Image>().color = ColorFromType(sequence[i]);
            currentPanels.Add(panel);
        }

        yield return new WaitForSeconds(displayTime);

        foreach (var panel in currentPanels)
            Destroy(panel);
        currentPanels.Clear();

        onComplete?.Invoke();
    }

    Color ColorFromType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Red:    return Color.red;
            case ColorType.Blue:   return Color.blue;
            case ColorType.Green:  return Color.green;
            case ColorType.Yellow: return Color.yellow;
            default:               return Color.white;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SequenceDisplayUI : MonoBehaviour
{
    public GameObject colorPrefab; // Un petit panel/Image pour chaque couleur
    public Transform container;    // Parent où placer les panels
    public float displayTime = 0.5f; // Temps entre chaque couleur

    private List<GameObject> currentPanels = new List<GameObject>();

    public void DisplaySequence(List<ColorType> sequence, System.Action onComplete)
    {
        StartCoroutine(DisplayRoutine(sequence, onComplete));
    }

    IEnumerator DisplayRoutine(List<ColorType> sequence, System.Action onComplete)
    {
        // Supprime l'ancien affichage
        foreach (var panel in currentPanels)
            Destroy(panel);
        currentPanels.Clear();

        // Affiche chaque couleur
        foreach (var color in sequence)
        {
            GameObject panel = Instantiate(colorPrefab, container);
            panel.GetComponent<Image>().color = ColorFromType(color); 
            currentPanels.Add(panel);
            yield return new WaitForSeconds(displayTime);
        }

        // Attend un petit moment avant de cacher
        yield return new WaitForSeconds(0.5f);

        // Masque tout
        foreach (var panel in currentPanels)
            Destroy(panel);
        currentPanels.Clear();

        // Callback vers le GameManager pour démarrer le spawn
        onComplete?.Invoke();
    }

    Color ColorFromType(ColorType type)
    {
        switch (type)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Blue: return Color.blue;
            case ColorType.Green: return Color.green;
            case ColorType.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }
}
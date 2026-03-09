using System.Collections.Generic;
using UnityEngine;

public class ColorSpawner : MonoBehaviour
{
    [Header("Platform Prefab")]
    public GameObject platformPrefab;
    public int platformPoolSize = 50;

    [Header("Spawn Zone")]
    public float minX = -2.3f;
    public float maxX =  2.3f;

    [Header("Platforms Per Row")]
    [Tooltip("Nombre minimum de plateformes par ligne horizontale.")]
    public int minPlatformsPerRow = 2;
    [Tooltip("Nombre maximum de plateformes par ligne horizontale.")]
    public int maxPlatformsPerRow = 3;

    [Header("Row Spacing")]
    [Tooltip("Écart vertical minimum entre deux lignes.")]
    public float minRowGap = 0.9f;
    [Tooltip("Écart vertical maximum entre deux lignes.")]
    public float maxRowGap = 1.4f;

    [Tooltip("Nombre de lignes à maintenir spawné au-dessus du joueur.")]
    public int spawnAheadRows = 16;

    [Tooltip("Distance sous l'écran avant de recycler.")]
    public float despawnBelowOffset = 2f;
    
    [Tooltip("Largeur du prefab de plateforme (Scale X). Utilisé pour éviter les chevauchements.")]
    public float platformWidth = 1.0f;

    
    private int rowCount = 0;
    
    private Transform playerTransform;
    private Camera mainCamera;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();
    private readonly List<GameObject> activePlatforms = new List<GameObject>();

    // Lot de couleurs mélangées : toutes les couleurs apparaissent dans chaque lot de 4
    private readonly Queue<ColorType> colorBatch = new Queue<ColorType>();

    private static readonly ColorType[] AllColors =
    {
        ColorType.Blue, ColorType.Red, ColorType.Green, ColorType.Yellow
    };

    private float nextRowY;

    void Start()
    {
        mainCamera      = Camera.main;
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (platformPrefab == null)
        {
            Debug.LogError("ColorSpawner: platformPrefab non assigné.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("ColorSpawner: aucun GameObject taggé 'Player' trouvé.");
            return;
        }

        InitPool();
        PrewarmPlatforms();
    }

    void Update()
    {
        if (playerTransform == null) return;

        SpawnRowsAheadOfPlayer();
        RecyclePlatformsBelowScreen();
    }

    // ─── Color Batch ──────────────────────────────────────────────────────────

    /// <summary>
    /// Retourne la prochaine couleur du lot mélangé.
    /// Remplit automatiquement un nouveau lot quand il est vide.
    /// </summary>
    ColorType NextColor()
    {
        if (colorBatch.Count == 0)
            RefillColorBatch();

        return colorBatch.Dequeue();
    }

    void RefillColorBatch()
    {
        ColorType[] shuffled = (ColorType[])AllColors.Clone();

        // Fisher-Yates shuffle
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        foreach (ColorType c in shuffled)
            colorBatch.Enqueue(c);
    }

    // ─── Pool ────────────────────────────────────────────────────────────────

    void InitPool()
    {
        for (int i = 0; i < platformPoolSize; i++)
        {
            GameObject go = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity, transform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go;
        }

        return Instantiate(platformPrefab, Vector3.zero, Quaternion.identity, transform);
    }

    void ReturnToPool(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }

    // ─── Spawning ─────────────────────────────────────────────────────────────

    void PrewarmPlatforms()
    {
        // Plateforme neutre plein écran sous le joueur
        SpawnNeutralStartPlatform();

        nextRowY = playerTransform.position.y + minRowGap;

        int rowsToFill = spawnAheadRows + Mathf.CeilToInt(mainCamera.orthographicSize * 2f / minRowGap);
        for (int i = 0; i < rowsToFill; i++)
            SpawnNextRow();
    }

    /// <summary>
    /// Spawne une plateforme neutre grise qui s'étend sur toute la largeur de l'écran.
    /// </summary>
    void SpawnNeutralStartPlatform()
    {
        GameObject go = GetFromPool();

        // Largeur = largeur totale de l'écran en unités monde
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect * 2f;

        // On scale la plateforme pour qu'elle prenne toute la largeur
        // en tenant compte de la scale de base du prefab
        Vector3 originalScale = go.transform.localScale;
        go.transform.localScale = new Vector3(
            screenWidth / platformWidth * originalScale.x,
            originalScale.y,
            originalScale.z
        );

        go.transform.position = new Vector3(0f, playerTransform.position.y - 0.7f, 0f);

        Platform platform = go.GetComponent<Platform>();
        platform?.SetNeutral();

        activePlatforms.Add(go);
    }


    void SpawnRowsAheadOfPlayer()
    {
        float threshold = playerTransform.position.y + spawnAheadRows * maxRowGap;

        while (nextRowY < threshold)
            SpawnNextRow();
    }

    /// <summary>
    /// Spawne une ligne de 2 à 3 plateformes réparties en zones horizontales distinctes.
    /// </summary>
    /// <summary>
    /// Spawne une ligne de 2 à 3 plateformes en quinconce par rapport à la ligne précédente.
    /// Les lignes impaires sont décalées de la moitié d'une zone pour éviter tout alignement vertical.
    /// </summary>
/// <summary>
/// Spawne une ligne de 2 à 3 plateformes garanties non superposées,
/// en quinconce par rapport à la ligne précédente.
/// </summary>
void SpawnNextRow()
{
    int count = Random.Range(minPlatformsPerRow, maxPlatformsPerRow + 1);

    // Génère des positions X valides avec espacement garanti
    List<float> positions = GenerateSpacedPositions(count);

    // Quinconce : lignes impaires décalées d'une demi-largeur de slot
    float slotWidth = (maxX - minX) / count;
    float offset    = (rowCount % 2 == 1) ? slotWidth * 0.5f : 0f;

    for (int i = 0; i < positions.Count; i++)
    {
        float x = Mathf.Clamp(positions[i] + offset, minX, maxX);
        SpawnPlatformAt(x, nextRowY, NextColor());
    }

    rowCount++;
    nextRowY += Random.Range(minRowGap, maxRowGap);
}

/// <summary>
/// Génère 'count' positions X aléatoires réparties dans des slots égaux,
/// triées et vérifiées pour garantir un espacement minimum de platformWidth entre elles.
/// </summary>
List<float> GenerateSpacedPositions(int count)
{
    float totalWidth = maxX - minX;
    float slotWidth  = totalWidth / count;
    // Espacement minimum = largeur de la plateforme + petite marge
    float minSpacing = platformWidth + 0.15f;

    List<float> positions = new List<float>(count);

    for (int i = 0; i < count; i++)
    {
        // Chaque plateforme est placée dans son slot, avec une variation aléatoire interne
        float slotMin = minX + i * slotWidth + minSpacing * 0.5f;
        float slotMax = minX + (i + 1) * slotWidth - minSpacing * 0.5f;

        // Si le slot est trop petit pour la plateforme, on centre
        if (slotMin >= slotMax)
            slotMin = slotMax = minX + (i + 0.5f) * slotWidth;

        positions.Add(Random.Range(slotMin, slotMax));
    }

    // Vérification finale : si deux positions sont trop proches malgré les slots,
    // on les écarte proprement
    positions.Sort();
    for (int i = 1; i < positions.Count; i++)
    {
        float gap = positions[i] - positions[i - 1];
        if (gap < minSpacing)
            positions[i] = Mathf.Min(positions[i - 1] + minSpacing, maxX);
    }

    return positions;
}

/// <summary>
/// Retourne un tableau d'indices de zones dans un ordre aléatoire.
/// </summary>
int[] ShuffledZoneOrder(int count)
{
    int[] zones = new int[count];
    for (int i = 0; i < count; i++) zones[i] = i;

    for (int i = zones.Length - 1; i > 0; i--)
    {
        int j = Random.Range(0, i + 1);
        (zones[i], zones[j]) = (zones[j], zones[i]);
    }

    return zones;
}

    void SpawnPlatformAt(float x, float y, ColorType color)
    {
        GameObject go = GetFromPool();
        go.transform.position = new Vector3(x, y, 0f);
        go.GetComponent<Platform>()?.SetColor(color);
        activePlatforms.Add(go);
    }

    // ─── Recycling ────────────────────────────────────────────────────────────

    void RecyclePlatformsBelowScreen()
    {
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize - despawnBelowOffset;

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.y < bottomY)
            {
                ReturnToPool(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ColorSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public int platformPoolSize = 15;

    [Header("Spawn Zone")]
    public float spawnYOffset = 6f;
    public float minX = -3f;
    public float maxX = 3f;
    public float minGap = 1.2f;
    public float maxGap = 2.2f;

    [Header("Difficulty")]
    public float gapIncreasePerPlatform = 0.02f;
    public float maxGapCap = 3.5f;

    private Transform playerTransform;
    private Camera mainCamera;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();
    private readonly List<GameObject> activePlatforms = new List<GameObject>();

    private float nextSpawnY;
    private float currentGap;

    private static readonly ColorType[] AvailableColors =
    {
        ColorType.Blue,
        ColorType.Red,
        ColorType.Green,
        ColorType.Yellow
    };

    void Start()
    {
        mainCamera = Camera.main;
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (platformPrefab == null)
        {
            Debug.LogError("ColorSpawner: platformPrefab is not assigned.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("ColorSpawner: no GameObject tagged 'Player' found.");
            return;
        }

        currentGap = minGap;
        nextSpawnY = playerTransform.position.y + 2f;

        InitPool();
        SpawnInitialPlatforms();
    }

    void Update()
    {
        if (playerTransform == null) return;

        RecyclePlatformsBelowScreen();
        SpawnPlatformsAheadOfPlayer();
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

    void SpawnInitialPlatforms()
    {
        // First platform directly under the player, same color for a safe start
        SpawnPlatformAt(playerTransform.position.y - 1f, playerTransform.position.x, forceSafeColor: true);

        for (int i = 0; i < platformPoolSize - 1; i++)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnPlatformsAheadOfPlayer()
    {
        float spawnThreshold = playerTransform.position.y + spawnYOffset;

        while (nextSpawnY < spawnThreshold)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        float x = Random.Range(minX, maxX);
        SpawnPlatformAt(nextSpawnY, x);

        currentGap = Mathf.Min(currentGap + gapIncreasePerPlatform, maxGapCap);
        nextSpawnY += Random.Range(minGap, currentGap);
    }

    void SpawnPlatformAt(float y, float x, bool forceSafeColor = false)
    {
        GameObject go = GetFromPool();
        go.transform.position = new Vector3(x, y, 0f);

        Platform platform = go.GetComponent<Platform>();
        if (platform != null)
        {
            ColorType color = forceSafeColor
                ? GetPlayerColor()
                : AvailableColors[Random.Range(0, AvailableColors.Length)];

            platform.SetColor(color);
        }

        activePlatforms.Add(go);
    }

    // ─── Recycling ────────────────────────────────────────────────────────────

    void RecyclePlatformsBelowScreen()
    {
        float bottomY = mainCamera.transform.position.y - mainCamera.orthographicSize - 1f;

        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].transform.position.y < bottomY)
            {
                ReturnToPool(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    ColorType GetPlayerColor()
    {
        var player = playerTransform?.GetComponent<PlayerController>();
        return player != null ? player.currentColor : AvailableColors[0];
    }
}

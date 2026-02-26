using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCollector player;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] exitPoints;
    [SerializeField] private GameUIManager uiManager;
    
    [Header("Component Prefabs")]
    [SerializeField] private GameObject componentPrefab;
    
    [Header("Color Data")]
    [SerializeField] private ComponentColorData[] availableColors;
    
    [Header("Game Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private int scorePerPair = 10;
    [SerializeField] private int victoryScoreThreshold = 50;
    
    [Header("Difficulty Settings")]
    [SerializeField] private float initialSpawnDelay = 1.5f;
    [SerializeField] private float minSpawnDelay = 0.3f;
    [SerializeField] private float initialFallSpeed = 2f;
    [SerializeField] private float maxFallSpeed = 5f;
    [SerializeField] private int scorePerDifficultyIncrease = 30;
    
    [Header("Spawn Count Progression")]
    [SerializeField] private int initialComponentsPerWave = 2;
    [SerializeField] private int scoreForThreeComponents = 30;
    [SerializeField] private int scoreForFourComponents = 60;
    [SerializeField] private int scoreForFiveComponents = 90;
    
    [Header("Timing Settings")]
    [SerializeField] private float observationTime = 2f;
    [SerializeField] private float travelTime = 3f;
    [SerializeField] private float pipeBlockDuration = 0.5f;
    [SerializeField] private float waveCooldown = 0.1f;
    
    [Header("Scene Management")]
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string victorySceneName = "SuccessGame1";
    
    private int currentLives;
    private int currentScore;
    private int difficultyLevel = 0;
    private ComponentColorData targetColor1;
    private ComponentColorData targetColor2;
    private bool gameRunning = false;
    
    private Dictionary<int, float> occupiedPipes = new Dictionary<int, float>();

    private void Awake()
    {
        Time.timeScale = 1f;
        Debug.Log("[GameManager] Awake - Time.timeScale reset to 1");
    }

    private void Start()
    {
        currentLives = maxLives;
        currentScore = 0;
        difficultyLevel = 0;
        gameRunning = false;
        
        if (uiManager != null)
        {
            uiManager.UpdateLives(currentLives);
            uiManager.UpdateScore(currentScore);
        }
        
        Debug.Log("========== GAME START ==========");
        Debug.Log($"[GameManager] Victory threshold: {victoryScoreThreshold}");
        Debug.Log($"[GameManager] Game Over scene: '{gameOverSceneName}'");
        Debug.Log($"[GameManager] Victory scene: '{victorySceneName}'");
        
        SetupNewPair();
        StartCoroutine(GameLoop());
    }

    private void OnDestroy()
    {
        Debug.Log("[GameManager] OnDestroy - Stopping all coroutines");
        StopAllCoroutines();
    }

    private void SetupNewPair()
    {
        targetColor1 = availableColors[Random.Range(0, availableColors.Length)];
        targetColor2 = availableColors[Random.Range(0, availableColors.Length)];
        
        while (targetColor1 == targetColor2)
        {
            targetColor2 = availableColors[Random.Range(0, availableColors.Length)];
        }
        
        player.SetTargetColors(targetColor1, targetColor2);
        
        Debug.Log($"[GameManager] New pair: {targetColor1.colorName} & {targetColor2.colorName}");
    }

    private IEnumerator GameLoop()
    {
        gameRunning = true;
        Debug.Log("[GameManager] GameLoop started");
        
        while (currentLives > 0)
        {
            yield return StartCoroutine(SpawnWave());
            
            float waveTimer = 0f;
            float maxWaveTime = 5f;
            
            while (waveTimer < maxWaveTime && !player.BothCollected() && gameRunning)
            {
                yield return new WaitForSeconds(0.1f);
                waveTimer += 0.1f;
            }
            
            if (player.BothCollected())
            {
                currentScore += scorePerPair;
                Debug.Log($"[GameManager] 🎉 Pair completed! Score: {currentScore}");
                
                CheckDifficultyIncrease();
                
                if (uiManager != null)
                {
                    uiManager.UpdateScore(currentScore);
                }
                
                yield return new WaitForSeconds(waveCooldown);
                
                if (currentLives > 0)
                {
                    SetupNewPair();
                }
            }
        }
        
        gameRunning = false;
        Debug.Log("[GameManager] GameLoop ended");
        EndGame();
    }

    private void EndGame()
    {
        bool isVictory = currentScore >= victoryScoreThreshold;
        
        GameData.SaveGameResult(currentScore, isVictory);
    
        if (isVictory)
        {
            LoadScene(victorySceneName);
        }
        else
        {
            LoadScene(gameOverSceneName);
        }
    }



    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }
      
        gameRunning = false;
        StopAllCoroutines();
        ComponentCollectable[] remainingComponents = FindObjectsOfType<ComponentCollectable>();
        foreach (var comp in remainingComponents)
        {
            if (comp != null)
            {
                Destroy(comp.gameObject);
            }
        }
        
        Time.timeScale = 1f;
        
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            
        }
    }

    private void CheckDifficultyIncrease()
    {
        int newDifficultyLevel = currentScore / scorePerDifficultyIncrease;
        
        if (newDifficultyLevel > difficultyLevel)
        {
            difficultyLevel = newDifficultyLevel;
        }
    }

    private int GetComponentsToSpawn()
    {
        if (currentScore >= scoreForFiveComponents)
            return 5;
        if (currentScore >= scoreForFourComponents)
            return 4;
        if (currentScore >= scoreForThreeComponents)
            return 3;
        
        return initialComponentsPerWave;
    }

    private float GetCurrentSpawnDelay()
    {
        float delay = initialSpawnDelay - (difficultyLevel * 0.2f);
        return Mathf.Max(delay, minSpawnDelay);
    }

    private float GetCurrentFallSpeed()
    {
        float speed = initialFallSpeed + (difficultyLevel * 0.5f);
        return Mathf.Min(speed, maxFallSpeed);
    }

    private IEnumerator SpawnWave()
    {
        if (!gameRunning)
        {
            yield break;
        }
        
        float currentSpawnDelay = GetCurrentSpawnDelay();
        int componentsToSpawn = GetComponentsToSpawn();
        
        List<int> usedPipesThisWave = new List<int>();
        
        for (int i = 0; i < componentsToSpawn; i++)
        {
            if (!gameRunning) yield break;
            
            int randomPipe = GetAvailablePipeExcluding(usedPipesThisWave);
            ComponentColorData randomColor = availableColors[Random.Range(0, availableColors.Length)];
            
            if (randomPipe != -1)
            {
                usedPipesThisWave.Add(randomPipe);
                occupiedPipes[randomPipe] = Time.time;
                
                StartCoroutine(SpawnComponent(randomPipe, randomColor));
                
                if (i < componentsToSpawn - 1)
                {
                    yield return new WaitForSeconds(currentSpawnDelay);
                }
            }
        }
    }

    private int GetAvailablePipeExcluding(List<int> excludedPipes)
    {
        List<int> pipesToRemove = new List<int>();
        foreach (var kvp in occupiedPipes)
        {
            if (Time.time - kvp.Value > pipeBlockDuration)
            {
                pipesToRemove.Add(kvp.Key);
            }
        }
        foreach (int pipe in pipesToRemove)
        {
            occupiedPipes.Remove(pipe);
        }
        
        List<int> availablePipes = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!occupiedPipes.ContainsKey(i) && !excludedPipes.Contains(i))
            {
                availablePipes.Add(i);
            }
        }
        
        if (availablePipes.Count > 0)
        {
            return availablePipes[Random.Range(0, availablePipes.Count)];
        }
        
        return -1;
    }

    private IEnumerator SpawnComponent(int pipeIndex, ComponentColorData colorData)
    {
        
        GameObject obj = Instantiate(componentPrefab, spawnPoints[pipeIndex].position, Quaternion.identity);
        ComponentCollectable comp = obj.GetComponent<ComponentCollectable>();
        comp.colorData = colorData;
        comp.SetFallSpeed(GetCurrentFallSpeed());
        comp.Initialize(pipeIndex, this);
        
        yield return new WaitForSeconds(observationTime);
        
        if (!gameRunning || obj == null)
        {
            if (obj != null) Destroy(obj);
            yield break;
        }
        
        obj.SetActive(false);
        yield return new WaitForSeconds(travelTime);
        
        if (!gameRunning || obj == null)
        {
            if (obj != null) Destroy(obj);
            yield break;
        }
        
        obj.transform.position = exitPoints[pipeIndex].position;
        obj.SetActive(true);
        comp.Activate();
    }

    public void OnComponentCollected(ComponentColorData colorData)
    {
        if (!gameRunning)
        {
            return;
        }
        
        bool accepted = player.TryCollect(colorData);
        
        if (!accepted)
        {
            currentLives--;
            
            if (uiManager != null)
            {
                uiManager.UpdateLives(currentLives);
            }
        }
    }
}

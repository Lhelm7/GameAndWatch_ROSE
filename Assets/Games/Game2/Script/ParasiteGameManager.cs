using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ParasiteGameManager : MonoBehaviour
{
    private const int ColorCount = 4;
    private const int PointsPerWave = 10;

    [Header("Wave Settings")]
    public List<WaveData> waves;
    private int currentWaveIndex = 0; 

    [Header("Sequence")]
    public List<ColorType> currentSequence = new List<ColorType>();
    private int currentInputIndex = 0;

    [Header("References")]
    public ParasiteSpawner spawner;
    public SequenceDisplayUI sequenceUI;
    public TMP_Text timerText;
    public TMP_Text scoreText;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject victory;

    [SerializeField] private AudioEventDispatcher audioEventDispatcher;

    [SerializeField] private GameObject music;
    [SerializeField] private Animator animator;
    private static readonly int Blink = Animator.StringToHash("Touch");
    
    private WaveData currentWave; 
    private float remainingTime; 
    private bool timerRunning = false;
    private int score = 0;
    private bool isGameOver = false;
    [SerializeField] private int Last_Wave = 0;
    void Start()
    {
        UpdateScoreDisplay();
        StartWave();
    }

    void Update()
    {
        if (!timerRunning || isGameOver) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (remainingTime <= 0f)
        {
            timerRunning = false;
            GameOver("Temps écoulé !");
        }
    }

    void StartWave()
    {
        currentWave = waves[currentWaveIndex];
        GenerateSequence(currentWave.sequenceLength);
        sequenceUI.DisplaySequence(currentSequence, OnSequenceDisplayed);
    }

    void OnSequenceDisplayed()
    {
        spawner.StartSpawning(currentWave);
        currentInputIndex = 0;
        StartTimer(currentWave.waveTime);
    }

    void StartTimer(float duration)
    {
        remainingTime = duration;
        timerRunning = true;
        UpdateTimerDisplay();
    }

    void StopTimer()
    {
        timerRunning = false;
        if (timerText != null)
            timerText.text = string.Empty;
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    /// <summary>Génère une séquence sans doublons si length <= 4, avec doublons sinon.</summary>
    void GenerateSequence(int length)
    {
        currentSequence.Clear();

        if (length <= ColorCount)
        {
            List<ColorType> pool = new List<ColorType>
            {
                ColorType.Blue, ColorType.Red, ColorType.Green, ColorType.Yellow
            };

            for (int i = pool.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (pool[i], pool[j]) = (pool[j], pool[i]);
            }

            for (int i = 0; i < length; i++)
                currentSequence.Add(pool[i]);
        }
        else
        {
            for (int i = 0; i < length; i++)
                currentSequence.Add((ColorType)Random.Range(0, ColorCount));
        }
    }

    /// <summary>Appelé par un Parasite quand le joueur clique dessus.</summary>
    public void OnParasiteTouched(ColorType touchedColor)
    {
        animator.SetTrigger("Touch");
        if (isGameOver) return;

        if (touchedColor == currentSequence[currentInputIndex])
        {
            
            currentInputIndex++;

            if (currentInputIndex >= currentSequence.Count)
                WaveComplete();
            audioEventDispatcher.PlayAudio(AudioType.Point);
        }
        else
        {
            gameover();
        }
    }

    void WaveComplete()
    {
        StopTimer();
        score += PointsPerWave;
        UpdateScoreDisplay();

        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log($"[GameManager] Jeu terminé ! Score final : {score}");
            Victory(); // <-- ajoute cet appel
            return;
        }

        StartWave();
    }


    void GameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        StopTimer();
        DestroyAllParasites();

        Debug.Log($"[GameManager] Game Over — {reason} | Score : {score}");
        // TODO : charger la scène de game over et passer le score via GameData.SaveGameResult(score, false)
    }

    void VictoryEnd()
    {
        audioEventDispatcher.PlayAudio(AudioType.Win);
        if (currentWaveIndex == Last_Wave) 
            Victory();
    }

    void DestroyAllParasites()
    {
        foreach (var p in FindObjectsByType<Parasite>(FindObjectsSortMode.None))
            Destroy(p.gameObject);
    }

    /// <summary>Retourne les couleurs de la séquence actuelle pour le spawner.</summary>
    public List<ColorType> GetAllowedColors() => currentSequence;
    
    private void gameover()
    {
        if (music != null)
            music.SetActive(false);
        audioEventDispatcher.PlayAudio(AudioType.Loose);
        Time.timeScale = 0f;

        if (gameOver!= null)
            gameOver.SetActive(true);
    }

    private void Victory()
    {
        if (music != null)
            music.SetActive(false);
        Time.timeScale = 0f;

        if (victory!= null)
            victory.SetActive(true);
    }
    
    public void RestartGame(string sceneName)
    {
        SceneManager.LoadScene("2ndGame");
    }

    public void Continue(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("3rdGame");
    }

    public void QuitGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}


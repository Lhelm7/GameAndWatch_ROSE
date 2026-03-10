using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameUIManager : MonoBehaviour
{
    [Header("Lives Display")]
    public Image[] lifeIcons;

    [Header("Objectives")]
    public Image objective1Image;
    public Image objective2Image;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over Text")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [SerializeField] private GameObject music;
    
    [Header("Events")]
    public UnityEvent OnDie;  

    private int currentScore;

    private void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void UpdateLives(int lives)
    {
        if (lifeIcons == null) return;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
                lifeIcons[i].enabled = i < lives;
        }

        if (lives <= 0)
        {
            Die();
        }
    }
    

    public void UpdateScore(int score)
    {
        currentScore = score;

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void Die()
    {
        Debug.Log("Player Died");

        Time.timeScale = 0f;
if (music != null)
    music.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        if (finalScoreText != null)
            finalScoreText.text = "Score : " + currentScore;

        OnDie?.Invoke();
    }

    public void RestartGame(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }
    

    public void QuitGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuChangeGame");
    }
}
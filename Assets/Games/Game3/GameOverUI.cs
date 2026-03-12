using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject gameOverPanel;

    [Header("Texts")]
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        gameOverPanel.SetActive(false);

        // Abonnement à l'event de mort du joueur
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.OnPlayerDied += ShowGameOver;
    }

    /// <summary>
    /// Affiche le panel Game Over avec le score final et le meilleur score.
    /// </summary>
    void ShowGameOver(int score)
    {

        finalScoreText.text = score + " m";
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Relance la scène. À brancher sur le bouton Retry.
    /// </summary>
    public void OnRetryPressed(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("3rdGame");
    }

    public void Quit(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuChangeGame");
    }
}
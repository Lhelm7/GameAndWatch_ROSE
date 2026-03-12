using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver2 : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    /// <summary>Affiche le score final du joueur sur le menu Game Over.</summary>
    public void DisplayScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score : {score}";
    }

    public void RetryGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("2ndGame");
    }

    public void QuitGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuChangeGame");
    }
}
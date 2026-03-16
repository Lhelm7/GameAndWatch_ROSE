using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public void RestartGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("2ndGame");
    }

    public void GoToHighScores()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HighScoreGame1"); // une scène par jeu
    }


}

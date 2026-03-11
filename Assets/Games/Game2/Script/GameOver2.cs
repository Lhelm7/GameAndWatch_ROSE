using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver2 : MonoBehaviour
{
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
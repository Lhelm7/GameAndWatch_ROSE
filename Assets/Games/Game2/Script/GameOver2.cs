using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver2 : MonoBehaviour
{
    public void RetryGame(string sceneName)
    {
        SceneManager.LoadScene("2ndGame");
    }
    
    public void QuitGame(string sceneName)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
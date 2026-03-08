using UnityEngine;
using UnityEngine.SceneManagement;
public class Victory : MonoBehaviour
{
    public void RestartGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("2ndGame");
    }

    public void QuitGame(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}

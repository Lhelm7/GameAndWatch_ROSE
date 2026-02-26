using UnityEngine;
using UnityEngine.SceneManagement;
public class VictoryMenuGame1 : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene("2ndGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

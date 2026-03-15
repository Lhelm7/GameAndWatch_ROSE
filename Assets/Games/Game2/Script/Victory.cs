using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Victory : MonoBehaviour

{
    [SerializeField] private AudioEventDispatcher audioeventdispatcher;

    private void Start()
    {
        if (audioeventdispatcher != null)
        {
            audioeventdispatcher.PlayAudio(AudioType.Victory);  
        }
    }

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

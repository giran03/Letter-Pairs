using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    string currentScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update() => currentScene = SceneManager.GetActiveScene().name;

    public void GoToScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void Button_MenuPlay() => GoToScene("_LevelSelect");

    public void Quit() => Application.Quit();

    public void Button_ReturnMainMenu() => SceneManager.LoadScene("_MainMenu");

    public string CurrentScene() => currentScene;

    public void PauseGame() => Time.timeScale = 0f;

    public void ResumeGame() => Time.timeScale = 1f;
}

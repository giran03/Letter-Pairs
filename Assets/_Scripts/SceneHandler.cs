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

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"currentScene: {currentScene}");

        // FOR DEMO ONLY
        if (Input.GetKeyDown(KeyCode.L))
            SkipScene();
    }

    public void GoToScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void Button_SFX()
    {
        SoundManager.Instance.PlaySFX("Button Select");
        Debug.Log($"Played button sfx");
    }

    public void Button_MenuPlay() => GoToScene("_LevelSelect");

    public void Quit() => Application.Quit();

    public void Button_ReturnMainMenu()
    {
        SceneManager.LoadScene("_MainMenu");
        SoundManager.Instance.PlayMusic("MenuBGM");
    }

    public void Button_CreditsScreen()
    {
        SceneManager.LoadScene("_Credits");
        SoundManager.Instance.PlayMusic("Credits BGM");
    }
    
    public void Button_RestartCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public string CurrentScene() => currentScene;

    public void PauseGame() => Time.timeScale = 0f;

    public void ResumeGame() => Time.timeScale = 1f;

    // FOR DEMO ONLY | can return null :)
    public void SkipScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}

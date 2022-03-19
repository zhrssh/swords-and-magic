using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager instance;
    private void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseScreen;

    public static bool isGamePaused = false;

    private void Start()
    {
        loadingScreen.SetActive(false);
        deathScreen.SetActive(false);
    }

    public void OnPlayButtonPressed()
    {
        StartCoroutine(LoadMainLevel());
    }

    private IEnumerator LoadMainLevel()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainLevel");
        while (!asyncOperation.isDone)
        {
            loadingScreen.SetActive(true);
            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("MenuLevel");
    }

    public void OnPauseButtonPressed()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Pause();
        }
    }

    public void OnResumeButtonPressed()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Resume();
        }
    }

    private void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    
    private void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DisplayDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    public void HideDeathScreen()
    {
        deathScreen.SetActive(false);
    }
}

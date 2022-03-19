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
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, bool additive = false, Action onComplete = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, additive, onComplete));
    }

    private IEnumerator LoadSceneAsync(string sceneName, bool additive, Action onComplete)
    {
        var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    public void UnloadScene(string sceneName, Action onComplete = null)
    {
        StartCoroutine(UnloadSceneAsync(sceneName, onComplete));
    }

    private IEnumerator UnloadSceneAsync(string sceneName, Action onComplete)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }
}
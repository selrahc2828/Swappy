using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneSwitch: MonoBehaviour
{
    public GameObject fonduAuNoir;
    public Scene NextScene;

    private void Start()
    {
        NextScene = SceneManager.GetSceneByBuildIndex(1);
    }
    public void OnPlayButtonClicked()
    {
        if (fonduAuNoir != null)
        {
            fonduAuNoir.SetActive(true);
        }
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(NextScene.name);
        SceneManager.UnloadSceneAsync(currentScene);
    }
}

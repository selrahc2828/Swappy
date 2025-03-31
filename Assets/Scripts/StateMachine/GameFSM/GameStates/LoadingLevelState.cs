using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;

public class LoadingLevelState : GameState
{
    public GameObject loadingMenuGO;
    public Slider loadingSlider;
    //Wait at least this amount of time :
    [SerializeField] private float minLoadingTime = 0.5f;
    
    private AsyncOperation asyncLoad;
    private float minTransitionTime;
    private float loadingProgress;


    public override void Enter()
    {
        minTransitionTime = Time.time + minLoadingTime;
        loadingMenuGO.SetActive(true);

        asyncLoad = SceneManager.LoadSceneAsync(fsm.selectedLevels[fsm.sceneID].BuildIndex, LoadSceneMode.Additive);
    }

    public override void Tick()
    {
        if (asyncLoad.isDone && Time.time >= minTransitionTime)
        {
            Scene activeScene = fsm.selectedLevels[fsm.sceneID].LoadedScene;
            SceneManager.SetActiveScene(activeScene);
            fsm.ChangeState(GetComponent<LevelInitializationState>());
        }

        loadingProgress = (Mathf.Abs(minTransitionTime - Time.time - minLoadingTime) / minLoadingTime + asyncLoad.progress) / 2f;
        loadingSlider.value = loadingProgress;
    }

    public override void Exit()
    {
        loadingMenuGO.SetActive(false);
    }
}
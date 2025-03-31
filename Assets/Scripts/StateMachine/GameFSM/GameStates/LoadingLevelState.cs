using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevelState : GameState
{
    public GameObject loadingMenuGO;
    //Wait at least this amount of time :
    [SerializeField] private float minLoadingTime = 0.5f;
    
    private AsyncOperation asyncLoad;
    private float minTransitionTime;
    
    
    public override void Enter()
    {
        minTransitionTime = Time.time + minLoadingTime;
        loadingMenuGO.SetActive(true);
        
        asyncLoad = SceneManager.LoadSceneAsync(fsm.selectedLevel.BuildIndex, LoadSceneMode.Additive);
    }

    public override void Tick()
    {
        if (asyncLoad.isDone && Time.time >= minTransitionTime)
        {
            fsm.ChangeState(GetComponent<LevelInitializationState>());
        }
        
        //todo : show loading bar/ image...
    }

    public override void Exit()
    {
        loadingMenuGO.SetActive(false);
    }
}
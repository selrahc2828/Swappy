using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance ? instance : FindObjectOfType<T>();

    protected void Awake()
    {
        if (!instance)
        { 
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        Initialize();
    }

    protected virtual void Initialize() {}

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}


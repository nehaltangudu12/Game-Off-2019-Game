using UnityEngine;

public class SingletonMB<T> : MonoBehaviour where T : class
{
    private static SingletonMB<T> instance;

    /// <summary>
    ///     Makes the object singleton not be destroyed automatically when loading a new scene.
    /// </summary>
    public bool DontDestroy;

    public static T Instance => instance as T;

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (DontDestroy)
                DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    protected virtual void Initialize()
    {
    }

    protected virtual void Shutdown()
    {
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
            Shutdown();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        if (instance == this)
        {
            instance = null;
            Shutdown();
        }
    }
}
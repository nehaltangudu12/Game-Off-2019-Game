using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<T> ();
        }

        DontDestroyOnLoad (this);
    }
}
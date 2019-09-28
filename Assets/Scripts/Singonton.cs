using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singonton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] instanceList = GameObject.FindObjectsOfType<T>();
                if (instanceList.Length > 1)
                    Debug.LogError("Singonton<>");
                else if (instanceList.Length == 1)
                    instance = instanceList[0];
                else
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return instance;
        }
        private set => instance = value;
    }
    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this as T;

        if (Instance != this)
            Destroy(gameObject);
    }
}
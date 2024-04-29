using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : SingletonSubMonobehaviour where T : SingletonSubMonobehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type type = typeof(T);
                instance = (T)FindObjectOfType(type);
                if (instance == null)
                {
                    instance = new GameObject(type.Name).AddComponent<T>();
                }
                instance.InitIfNeeded();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (this == Instance)
        {
            return;
        }
        Destroy(gameObject);
    }

    protected virtual void Start() { }

    protected virtual void OnEnable()
    {
        if (this == Instance)
        {
            instance.InitIfNeeded();
        }
    }

    protected virtual void OnDestroy()
    {
        if (this == Instance)
        {
            instance.DeinitIfNeeded();
            instance = null;
        }
    }
}
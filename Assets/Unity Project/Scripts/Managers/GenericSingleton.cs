using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    protected static T m_Instance;
    public static T Instance
    {
        get
        {
            // If there's no private instance...
            if (m_Instance == null)
            {
                // Search for it!
                m_Instance = FindObjectOfType<T>();

                // And if we don't find it,
                if (m_Instance == null)
                {
                    GameObject newObject = new();
                    newObject.name = typeof(T).Name;
                    m_Instance = newObject.AddComponent<T>();
                    DontDestroyOnLoad(newObject);
                }
            }

            return m_Instance;
        }
    }

    protected void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

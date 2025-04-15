using UnityEngine;
using Fusion;

public class Singleton<T> : NetworkBehaviour where T : Component
{
    private static T m_instance;
    public static T Instance
    {
        get
        {
            if(m_instance is null)
            {
                m_instance = FindFirstObjectByType<T>();

                if(m_instance is null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    m_instance = obj.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }

    public virtual void Awake()
    {
        if(m_instance is null)
        {
            m_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

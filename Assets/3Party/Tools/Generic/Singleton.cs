using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T temp = FindObjectOfType<T>();
                if (temp != null)
                    instance = temp;
                else
                {
                    instance = new GameObject().AddComponent<T>();
                    instance.gameObject.name = instance.GetType().Name;
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        instance = this as T;
        DontDestroyOnLoad(instance);
    }

    public static bool Exists => instance;
}


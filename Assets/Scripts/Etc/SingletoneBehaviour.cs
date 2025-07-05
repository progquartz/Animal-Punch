using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    protected bool IsDestroyOnLoad { get; set; } = false;
    private static T s_instance;
    private static bool isInitialized = false;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindFirstObjectByType<T>();

                if (s_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    s_instance = singletonObject.AddComponent<T>();
                }
            }

            return s_instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this as T;
        }

        if (!isInitialized)
        {
            Init();    
            isInitialized = true;
        }

        if (!IsDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void Init()
    {
        // 상속된 클래스에서 사용
    }

    protected virtual void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
            isInitialized = false;
        }
    }

    protected void ChangeParentToManagers()
    {
        Transform managerTransform = GameObject.Find("Managers").transform;
        gameObject.transform.SetParent(managerTransform, false);
    }
}

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    [SerializeField] private bool dontDestroyOnLoad = false;

    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            if (dontDestroyOnLoad)
            {
                if (transform.parent != null)
                    transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }
}

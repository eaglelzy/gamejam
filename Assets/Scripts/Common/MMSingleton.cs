using UnityEngine;

namespace MoreMountains.Tools
{
    public class MMSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Current => _instance;
        protected static T _instance;

        [SerializeField]
        public bool dontDestroyOnLoad = true;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject obj = new() {
                            name = typeof(T).Name + "_AutoCreated"
                        };
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (_instance == null)
            {
                _instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(transform.gameObject);
                }
            }
            else
            {
                if (this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

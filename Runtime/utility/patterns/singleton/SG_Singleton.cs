using SGLib.Utility.Management.Component.Capabilities;
using UnityEngine;

namespace SGLib.Utility.Patterns.Singleton
{
    public abstract class SG_Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual bool Persistent => true;
        protected static T instance;
        private static bool isQuitting;

        public static T Instance
        {
            get
            {
                if (isQuitting) return null;

                if (instance != null) return instance;

                instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying) return;

            if (instance == null)
            {
                instance = this as T;

                if (Persistent) DontDestroyOnLoad(gameObject);

                OnSingletonAwake();
                return;
            }

            if (instance != this) Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (instance != this) return;
            instance = null;
        }

        protected virtual void OnApplicationQuit() => isQuitting = true;

        protected virtual void OnSingletonAwake() { }
    }
}


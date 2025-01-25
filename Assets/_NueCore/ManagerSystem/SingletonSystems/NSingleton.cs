using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.ManagerSystem.SingletonSystems
{
    public interface INSingleton
    {
        void InitSingleton();
    }
    public class NSingleton<T> : NSingletonBase<NSingleton<T>>, INSingleton where T : MonoBehaviour,INSingleton
    {
         private static T _instance;
         
        [FoldoutGroup("Singleton Settings")]
        [FoldoutGroup("Singleton Settings")][SerializeField] private bool dontDestroyLoad;
        
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance != null) return _instance;
                
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                return _instance;
            }
        }
        
        public void InitSingleton()
        {
            if (_instance == null)
            {
                _instance = gameObject.GetComponent<T>();
                if (dontDestroyLoad)
                    SetDontDestroyOnLoad();
                SAwake();
                return;
            }
            
            if (this == _instance)
            {
                if (dontDestroyLoad)
                    SetDontDestroyOnLoad();
                SAwake();
                return;
            }
            
            Destroy(gameObject);
        }

        protected virtual void SAwake()
        {
            
        }
        
        private void SetDontDestroyOnLoad()
        {
            if (!dontDestroyLoad) return;
            if (transform.parent != null)
                transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        
    }
}
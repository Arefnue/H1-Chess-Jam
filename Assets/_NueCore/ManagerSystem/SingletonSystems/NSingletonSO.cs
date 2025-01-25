using _NueCore.Common.NueLogger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _NueCore.ManagerSystem.SingletonSystems
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);
                    _instance = Resources.Load<T>(type.Name);
                }

                if (_instance == null)
                {
                    var type = typeof(T);
                    $"Couldn't find {type.Name} in resources folder".NLog(Color.red);
                    return null;
                }

                return _instance;
            }
        }


#if UNITY_EDITOR
        [Button,FoldoutGroup("Editor")]
        protected virtual void ResetInstance()
        {
            _instance = null;
        }
#endif
    }
}
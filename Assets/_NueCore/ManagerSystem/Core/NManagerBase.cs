using _NueCore.Common.NueLogger;
using _NueCore.ManagerSystem.SingletonSystems;
using UnityEngine;

namespace _NueCore.ManagerSystem.Core
{
    public abstract class NManagerBase: MonoBehaviour,INInit
    {
        #region Cache
        protected bool IsManagerAwaken { get; private set; }
        protected bool IsManagerStarted { get; private set; }
        

        #endregion
        
        #region Manager Setup
        public virtual void NAwake()
        {
            if (IsManagerAwaken) return;
            
        }
        public virtual void NStart()
        {
            if (IsManagerStarted) return;
        }
        public virtual void NReset()
        {
            $"<<RESET>> {gameObject.name}".NLog(Color.red);
        }
        protected T InitSingleton<T>()
        {
            SingletonExtension.Remove<T>();
            return SingletonExtension.Get<T>();
        }

        public virtual void SetGlobalManager()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        #endregion
        
        #region Unity Setup
        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnDestroy()
        {
            
        }
        #endregion
    }
}
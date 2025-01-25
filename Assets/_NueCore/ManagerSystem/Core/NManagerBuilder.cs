using System.Collections.Generic;
using _NueCore.Common.NueLogger;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _NueCore.ManagerSystem.Core
{
    [DefaultExecutionOrder(-1)]
    public class NManagerBuilder : MonoBehaviour
    {
        [SerializeField,TabGroup("References")] private NManagersCatalog coreManagersData;
        [SerializeField,TabGroup("References")] private NManagersCatalog localManagerCatalog;
        [SerializeField,TabGroup("References")] private List<NManagerBase> sceneManagersList;
        
        [SerializeField,TabGroup("Events")] private UnityEvent initFinishedUnityEvent;
        [SerializeField,TabGroup("Events")] private UnityEvent buildFinishedUnityEvent;

        #region Cache

        [ShowInInspector,ReadOnly,TabGroup("Base","Debug",order:99)]
        private static List<NManagerBase> SpawnedCoreManagerList { get;  set; }= new();
        private static List<NManagerBase> SpawnedExternalManagerList { get;  set; }= new();

        private static bool _isManagersInitOnce;
        private static bool _isManagersStartedOnce;

        #endregion
        
        #region Setup
        private void Awake()
        {
            AwakeCoreManagers();
            AwakeExternalManagers();
            AwakeSceneManagers();
            initFinishedUnityEvent?.Invoke();
        }
        
        private void Start()
        {
            StartCoreManagers();
            StartExternalManagers();
            StartSceneManagers();
            buildFinishedUnityEvent?.Invoke();
        }
        #endregion

        #region Local Methods
        private void AwakeCoreManagers()
        {
            if (!coreManagersData) return;
            if (_isManagersInitOnce) return;
            _isManagersInitOnce = true;
            SpawnedCoreManagerList.Clear();
            
            for (int i = 0; i < coreManagersData.ManagerList.Count; i++)
            {
                var managerPrefab = coreManagersData.ManagerList[i];
                if (managerPrefab == null)
                {
                    "<<CORE>> Missing Core Manager Prefab Appears".NLog(Color.red);
                    continue;
                }
                var cloneManager = Instantiate(managerPrefab, transform);
                SpawnedCoreManagerList.Add(cloneManager);
            }
            
          
            for (int i = 0; i < SpawnedCoreManagerList.Count; i++)
            {
                var manager = SpawnedCoreManagerList[i];
                if (manager == null)
                {
                    "<<CORE>> Missing Core Manager Appears".NLog(Color.red);
                    continue;
                }
                
                manager.NAwake();
                manager.SetGlobalManager();
                $"<<CORE>> {manager.name} AWAKEN".NLog(Color.green);
            }
        }
        
        private void AwakeExternalManagers()
        {
            if (localManagerCatalog == null)
                return;
            SpawnedExternalManagerList.Clear();
            for (int i = 0; i < localManagerCatalog.ManagerList.Count; i++)
            {
                var managerPrefab = localManagerCatalog.ManagerList[i];
                if (managerPrefab == null)
                {
                    "<<CORE>> Missing External Manager Prefab Appears".NLog(Color.red);
                    continue;
                }
                var cloneManager = Instantiate(managerPrefab, transform);
                SpawnedExternalManagerList.Add(cloneManager);
            }

            
            for (int i = 0; i < SpawnedExternalManagerList.Count; i++)
            {
                var manager = SpawnedExternalManagerList[i];
               
                if (manager == null)
                {
                    "<<CORE>> Missing External Manager Appears".NLog(Color.red);
                    continue;
                }
                
                manager.NAwake();
                $"<<External>> {manager.name} AWAKEN".NLog(Color.yellow);
            }
        }
        
        private void AwakeSceneManagers()
        {
            for (int i = 0; i < sceneManagersList.Count; i++)
            {
                var manager = sceneManagersList[i];
                if (manager == null)
                {
                    "<<CORE>> Missing Scene Manager Appears".NLog(Color.red);
                    continue;
                }
                
                manager.NAwake();
                $"<<SCENE>> {manager.name} AWAKEN".NLog(Color.yellow);
            }
        }
        private void StartCoreManagers()
        {
            if (!coreManagersData) return;
            if (_isManagersStartedOnce) return;
            _isManagersStartedOnce = true;
            
            for (int i = 0; i < SpawnedCoreManagerList.Count; i++)
            {
                var manager = SpawnedCoreManagerList[i];

                if (manager == null)
                {
                    "<<CORE>> Missing Core Manager Appears".NLog(Color.red);
                    continue;
                }

                manager.NStart();  
                $"<<CORE>> {manager.name} STARTED".NLog(Color.green);
            }
        }
        
        private void StartExternalManagers()
        {
            if (localManagerCatalog == null)
                return;
            for (int i = 0; i < SpawnedExternalManagerList.Count; i++)
            {
                var manager = SpawnedExternalManagerList[i];

                if (manager == null)
                {
                    "<<CORE>> Missing External Manager Appears".NLog(Color.red);
                    continue;
                }

                manager.NStart();
                $"<<External>> {manager.name} STARTED".NLog(Color.yellow);
            }
        }
        private void StartSceneManagers()
        {
            for (int i = 0; i < sceneManagersList.Count; i++)
            {
                var manager = sceneManagersList[i];

                if (manager == null)
                {
                    "<<CORE>> Missing Scene Manager Appears".NLog(Color.red);
                    continue;
                }

                manager.NStart();
                $"<<SCENE>> {manager.name} STARTED".NLog(Color.yellow);
            }
        }
        #endregion
        
        //TODO State machine
    }
}

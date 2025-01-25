using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _NueCore.SceneSystem
{
    public class NSceneChanger : MonoBehaviour
    {
        [SerializeField] private bool useTransitionTarget;
        [SerializeField,HideIf(nameof(useTransitionTarget))] private SceneEnums targetScene;
        [SerializeField] private bool changeAtStart;
        [SerializeField] private bool isAsync;
        [SerializeField] private bool useFader;
        [SerializeField,ShowIf(nameof(isAsync))] private UnityEvent<float> asyncProgressUnityEvent;

        public SceneEnums TargetScene => useTransitionTarget ? SceneStatic.TransitionTarget: targetScene;

        private void Start()
        {
            if (changeAtStart)
                ChangeScene();
        }

        public void ChangeScene()
        {
            if (isAsync)
            {
                if (useFader)
                {
                    SceneStatic.ChangeSceneAsyncWithFader(TargetScene);
                }
                else
                {
                    var asyncOps = SceneStatic.ChangeSceneAsync(TargetScene);
                    StartCoroutine(LoadAsyncSceneRoutine(asyncOps));
                }
               
            }
            else
            {
                if (useFader)
                {
                    SceneStatic.ChangeSceneWithFader(TargetScene);
                }
                else
                {
                    SceneStatic.ChangeScene(TargetScene);
                }
            }
        }
        
        private IEnumerator LoadAsyncSceneRoutine(AsyncOperation asyncOperation)
        {
            while (!asyncOperation.isDone)
            {
                asyncProgressUnityEvent?.Invoke(asyncOperation.progress);
                yield return null;
            }
        }

    }
}